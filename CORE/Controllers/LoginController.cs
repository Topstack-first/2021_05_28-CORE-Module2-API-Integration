using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CORE.Models;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cors;
using CORE.Encryption;

namespace CORE.Controllers
{
    [Route("api/UserLogin")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class LoginController : Controller
    {
        private readonly COREContext _context;
        private readonly JWTSettings _jwtsettings;
        private readonly EncryptSalt _salt;
        private readonly MD5encrypt _enc;

        public LoginController(COREContext context, IOptions<JWTSettings> jwtsettings, IOptions<EncryptSalt> salt)
        {
            _context = context;
            _jwtsettings = jwtsettings.Value;
            _salt = salt.Value;
            _enc = new MD5encrypt(_salt);
        }
        #region Authenticate&Authorization
        [HttpPost]
        public async Task<IActionResult> UserLogin([FromBody] Login login)
        {
            if (login != null)
            {
                try
                {
                    var encpwd = _enc.MD5Convert(login.UserPassword);

                    User user = await _context.Users
                                    .Where
                                    (usr => (usr.UserName == login.UserNameEmail || usr.UserEmail == login.UserNameEmail) && usr.UserPassword == encpwd)
                                    .FirstOrDefaultAsync();

                    if (user == null)
                    {
                        User checkUserName = await _context.Users.Where(usr => usr.UserName == login.UserNameEmail || usr.UserEmail == login.UserNameEmail).FirstOrDefaultAsync();
                        if (checkUserName == null)
                        {
                            return StatusCode(401, Json(new APIErrReturn { Error = "Username does not exists!" }));
                        }
                        else
                        {
                            return StatusCode(401, Json(new APIErrReturn { Error = "Invalid Password!" }));
                        }
                    }
                    if (user.DeleteStatus == true)
                    {
                        return StatusCode(401, Json(new APIErrReturn { Error = "Account has been removed by admin, kindly contact admin for more info!" }));
                    }

                    if (user.UserApprovalStatus == "Pending")
                    {
                        return StatusCode(401, Json(new APIErrReturn { Error = "Account is not yet approved by Admin!" }));

                    }
                    if (user.UserApprovalStatus == "Rejected")
                    {
                        return StatusCode(401, Json(new APIErrReturn { Error = "Account approval is rejected by Admin!" }));

                    }
                    if (user.UserAccountStatus == false)
                    {
                        return StatusCode(401, Json(new APIErrReturn { Error = "Account is not Active!" }));

                    }
                    

                    //Add user refreshToken into table RefreshToken based on UserID 
                    RefreshToken refreshToken = GenerateRefreshToken();
                    user.RefreshTokens.Add(refreshToken);
                    await _context.SaveChangesAsync();
                    UserWithToken userWithToken = new UserWithToken(user);
                    userWithToken.RefreshToken = refreshToken.Token;
                    if (userWithToken == null)
                    {
                        return StatusCode(401, Json(new APIErrReturn { Error = "Something Went Wrong in the server!" }));
                    }
                    // JWT create and signage GenerateAccessToken
                    userWithToken.AccessToken = GenerateAccessToken(user.UserId, (int)user.RoleId);
                    return Ok(userWithToken);
                }
                catch (Exception ex)
                {
                    return StatusCode(401, Json(new APIErrReturn { Error = "Something Went Wrong in the server! = " + ex.ToString() }));
                }

            }
            else
            {
                return StatusCode(401, Json(new APIErrReturn { Error = "Request Body is empty!" }));
            }



        }


        private string GenerateAccessToken(int UserID, int RoleID)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtsettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, Convert.ToString(UserID)),
                    new Claim(ClaimTypes.Role, Convert.ToString(RoleID))
                }),
                //Expires = DateTime.UtcNow.AddHours(4),//for live
                // Expires = DateTime.UtcNow.AddSeconds(7),//for testing
                Expires = DateTime.UtcNow.AddHours((double)_jwtsettings.ExpiryDuration),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
        private RefreshToken GenerateRefreshToken()
        {
            RefreshToken refreshToken = new RefreshToken();
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                refreshToken.Token = Convert.ToBase64String(randomNumber);
            }
            refreshToken.ExpiryDate = DateTime.UtcNow.AddMonths(6);

            return refreshToken;
        }


        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshRequest refreshRequest)
        {
            //To extract User ID from User current JWT
            User user = GetUserFromAccessToken(refreshRequest.AccessToken);
            //To validate the Users JWT expiration date
            if (user != null && ValidateRefreshToken(user, refreshRequest.RefreshToken))
            {
                UserWithToken userWithToken = new UserWithToken(user);
                userWithToken.AccessToken = GenerateAccessToken(user.UserId, (int)user.RoleId);

                return Ok(userWithToken);
            }
            return StatusCode(401, "Something just went wrong!");
        }

        private bool ValidateRefreshToken(User user, string refreshToken)
        {
            RefreshToken refreshTokenUser = _context.RefreshTokens.Where(rt => rt.Token == refreshToken)
                                        .OrderByDescending(rt => rt.ExpiryDate)
                                        .FirstOrDefault();

            if (refreshTokenUser != null && refreshTokenUser.UserId == user.UserId && refreshTokenUser.ExpiryDate > DateTime.UtcNow)
            {
                return true;
            }
            return false;
        }

        private User GetUserFromAccessToken(string accessToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtsettings.SecretKey);
            var TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false
            };

            SecurityToken securityToken;
            var principle = tokenHandler.ValidateToken(accessToken, TokenValidationParameters, out securityToken);
            JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;
            //if (jwtSecurityToken != null && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature, StringComparison.InvariantCultureIgnoreCase))
            if (jwtSecurityToken != null)
            {
                var userId = principle.FindFirst(ClaimTypes.Name)?.Value;

                return _context.Users.Where(usr => usr.UserId == Convert.ToInt32(userId)).FirstOrDefault();
            }
            return null;
        }


        #endregion
        #region CheckFirstTimelogin
        [HttpPost("CheckFirstTimeLogin")]
        public async Task<IActionResult> CheckFirstTimeLogin([FromBody] Login userlogin)
        {
            var GetUserIdFromUserName = await _context.Users.Where(x => x.UserName == userlogin.UserNameEmail).FirstOrDefaultAsync();
            if (GetUserIdFromUserName == null) 
            {
                return StatusCode(202, "Username does not exists!");
            }
            var checkfirstimelogin = _context.RefreshTokens.Where(x => x.UserId == GetUserIdFromUserName.UserId).Count();
            if (checkfirstimelogin < 1) 
            {
                var encpwd = _enc.MD5Convert("default password");
                if (encpwd == GetUserIdFromUserName.UserPassword)
                {
                    return StatusCode(200, "User have never logged in before, this is the first time login!");
                }
                else 
                {
                    return StatusCode(200, "User have never logged in before, however the password is not default!");
                }
            }
            return  StatusCode(200, "User Login Count : "+checkfirstimelogin);
        }
        #endregion
    }
}
