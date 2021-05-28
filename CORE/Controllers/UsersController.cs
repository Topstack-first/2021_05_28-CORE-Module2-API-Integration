using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CORE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Json;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Cors;
using CORE.Encryption;
using System.IO;
using System.Reflection;

namespace CORE.Controllers
{
    [Authorize]
    [Route("api")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class UsersController : Controller
    {
        private readonly COREContext _context;
        private readonly EncryptSalt _salt;
        private readonly MD5encrypt _enc;

        public UsersController(COREContext context, IOptions<EncryptSalt> salt)
        {
            _context = context;
            _salt = salt.Value;
            _enc = new MD5encrypt(_salt);
        }

        [Authorize(Roles = ("1"))]
        [HttpGet("GetAllUser")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var cOREContext = _context.Users
                    .Select(usr => new { usr.UserId, usr.UserName, usr.UserEmail, usr.FirstName, usr.LastName, usr.RoleId, usr.DepartmentId, usr.DeleteStatus, usr.JobTitle, usr.UserBiography, usr.DocumentAttached, usr.UserPost, usr.UserAccountStatus, usr.UserApprovalStatus, usr.UserProfilePic, usr.CreatedBy, usr.CreatedAt })
                    .Where(usr => usr.DeleteStatus != true)
                    .OrderByDescending(a=>a.CreatedAt)
                    .ToListAsync(); 
                
                return Ok(await cOREContext);
            }
            catch(Exception ex) 
            {
                return StatusCode(404, ex.ToString());
            }
            
        }

        [HttpPost("GetUserInfoById")]
        public async Task<IActionResult> GetUserInfoById([FromBody] UserID UID)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var RoleID = identity.FindFirst(ClaimTypes.Role)?.Value;
            var UserID = identity.FindFirst(ClaimTypes.Name)?.Value;
            if (RoleID.ToString() == "1" || UID.UID.ToString() == UserID)
            {
                var cOREContext = _context.Users.Select(usr => new { usr.UserId, usr.UserName, usr.UserEmail, usr.FirstName, usr.LastName, usr.RoleId, usr.DepartmentId, usr.JobTitle, usr.UserBiography, usr.DeleteStatus, usr.DocumentAttached, usr.UserPost, usr.UserAccountStatus, usr.UserApprovalStatus, usr.UserProfilePic, usr.CreatedBy, usr.CreatedAt }).Where(usr => usr.UserId == UID.UID).FirstOrDefaultAsync();
                if (cOREContext != null) 
                {
                    return Ok(await cOREContext);
                }
                return StatusCode(202, new APIErrReturn { Error = "There are no user that exists with the given id :"+UID.UID});
            }
            return StatusCode(202, new APIErrReturn { Error = "You are not allowed to access other user's information!. Only user with admin privileges can!." });
                
        }

        [AllowAnonymous]
        [HttpGet("GetDepartments")]
        public async Task<IActionResult> GetDepartments()
        {
            var departments = await _context.Departments.Select(d => new { d.DepartmentId, d.Name }).ToListAsync();
            return Ok(departments);
        }

        [HttpGet("GetRolesAndPermission")]
        public async Task<IActionResult> GetRolesAndPermission()
        {
            var Role = await _context.Roles.Select(r => new { r.RoleId, r.RoleName, r.RolePermission }).ToListAsync();
            var Permission = await _context.Permissions.Select(p => new { p.PermissionId, p.PermissionName,p.BitWiseValue }).ToListAsync();
            return Ok(new { Role, Permission });
        }

        [Authorize(Roles = ("1"))]
        [HttpPost("CreateNewUserbyAdmin")]
        public async Task<IActionResult> CreateNewUserbyAdmin([FromBody] User user)
        {

            if (user != null)
            {
                if (user.UserName == null || user.UserName == "") { return StatusCode(202, new APIErrReturn { Error = "User Name data is empty/null!" }); }
                else
                {
                    User CheckUserName = await _context.Users.Where(cu => string.Equals(cu.UserName, user.UserName) || string.Equals(user.UserEmail, cu.UserEmail) || string.Equals(user.UserName, cu.UserEmail)).FirstOrDefaultAsync();
                    if (CheckUserName != null)
                    {
                        if (string.Equals(user.UserName, CheckUserName.UserName, StringComparison.OrdinalIgnoreCase))
                            return StatusCode(202, new APIErrReturn { Error = "Username already exists!" });
                        if (string.Equals(user.UserEmail, CheckUserName.UserEmail, StringComparison.OrdinalIgnoreCase))
                            return StatusCode(202, new APIErrReturn { Error = "Email has been used by other user, kindly use different email!" });
                    }
                }
                if (user.UserPassword == null || user.UserPassword == "") { return StatusCode(202, new APIErrReturn { Error = "User Password data is empty/null!" }); }
                if (user.DepartmentId == null || user.DepartmentId == 0) { return StatusCode(202, new APIErrReturn { Error = "User department id data is empty/null!" }); }
                if (user.RoleId == null || user.RoleId == 0) { return StatusCode(202, new APIErrReturn { Error = "Role data is 0/null!" }); }
                if (user.UserEmail == null || user.UserEmail == "") { return StatusCode(202, new APIErrReturn { Error = "UserEmail data is empty/null!" }); }
                user.UserPassword = _enc.MD5Convert(user.UserPassword);
                user.UserApprovalStatus = "Approved";
                user.UserAccountStatus = true;
                user.CreatedBy = "SYSTEM";
                user.DeleteStatus = false;
                //user.DocumentAttached = 0;
            }
            _context.Add(user);
            await _context.SaveChangesAsync();
            return Ok(await _context.Users.Where(ai => ai.UserName == user.UserName && ai.UserPassword == user.UserPassword).FirstAsync());
        }

        [AllowAnonymous]
        [HttpPost("CreateNewUserbyUser")]
        public async Task<IActionResult> CreateNewUserbyUser([FromBody] User user)
        {

            if (user != null)
            {
                if (user.UserName == null || user.UserName == "") { return StatusCode(202, new APIErrReturn { Error = "User Name data is empty/null!" }); }
                else
                {
                    User CheckUserName = await _context.Users.Where(cu => string.Equals(cu.UserName, user.UserName) || string.Equals(user.UserEmail, cu.UserEmail) || string.Equals(user.UserName, cu.UserEmail)).FirstOrDefaultAsync();
                    if (CheckUserName != null)
                    {
                        if (string.Equals(user.UserName, CheckUserName.UserName, StringComparison.OrdinalIgnoreCase))
                            return StatusCode(202, new APIErrReturn { Error = "Username already exists!" });
                        if (string.Equals(user.UserEmail, CheckUserName.UserEmail, StringComparison.OrdinalIgnoreCase))
                            return StatusCode(202, new APIErrReturn { Error = "Email has been used by other user, kindly use different email!" });
                    }
                }
                if (user.UserPassword == null || user.UserPassword == "") { return StatusCode(202, new APIErrReturn { Error = "User Password data is empty/null!" }); }
                if (user.FirstName == null || user.FirstName == "") { return StatusCode(202, new APIErrReturn { Error = "FirstName data is empty/null!" }); }
                if (user.LastName == null || user.LastName == "") { return StatusCode(202, new APIErrReturn { Error = "LastName data is empty/null!" }); }
                if (user.UserEmail == null || user.UserEmail == "") { return StatusCode(202, new APIErrReturn { Error = "UserEmail data is empty/null!" }); }
                if (user.DepartmentId == null || user.DepartmentId == 0) { return StatusCode(202, new APIErrReturn { Error = "Department data is 0/null!" }); }
                user.UserPassword = _enc.MD5Convert(user.UserPassword);
                user.UserApprovalStatus = "Pending";
                user.UserAccountStatus = false;
                user.CreatedBy = "USER";
                user.DeleteStatus = false;
                user.DocumentAttached = 0;
                user.RoleId = 4;
                user.JobTitle = "";
                user.UserBiography = "";
                user.UserProfilePic = "";
                user.UserPost = 0;
            }
            _context.Add(user);
            await _context.SaveChangesAsync();
            return Ok(await _context.Users.Where(ai => ai.UserName == user.UserName && ai.UserPassword == user.UserPassword).FirstAsync());
        }


        [HttpPost("UpdateUserInfobyUserId")]
        public async Task<IActionResult> UpdateUserInfobyUserId([FromBody] User user)
        {

            if (user != null)
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                var RoleID = identity.FindFirst(ClaimTypes.Role)?.Value;
                var UserID = identity.FindFirst(ClaimTypes.Name)?.Value;

                User UserToUpdate = await _context.Users.Where(cu => cu.UserId == user.UserId).FirstOrDefaultAsync();
                
                if (UserToUpdate != null)
                {
                    if (RoleID.ToString() == "1" || UserToUpdate.UserId.ToString() == UserID)
                    {
                        if (!string.IsNullOrEmpty(user.UserName))
                        {
                            User CheckUserName = await _context.Users.Where(chkusr => chkusr.UserName == user.UserName).FirstOrDefaultAsync();
                            if (CheckUserName != null && (CheckUserName.UserId != user.UserId))
                            {

                                return StatusCode(403, "UserName already exists!, kindly use other username that is not in use!");
                            }
                            else 
                            {
                                UserToUpdate.UserName = user.UserName;
                            }
                        }
                        
                        //if (!string.IsNullOrEmpty(user.UserPassword))
                        //{
                        //    UserToUpdate.UserPassword = _enc.MD5Convert(user.UserPassword); ;
                        //}
                        //else
                        //{
                        //    return StatusCode(403, "The user's password is empty!");
                        //}
                        
                        if (!string.IsNullOrEmpty(user.FirstName))
                        {
                            UserToUpdate.FirstName = user.FirstName;
                        }
                        else
                        {
                            return StatusCode(403, "The user's First Name is empty!");
                        }
                        
                        if (!string.IsNullOrEmpty(user.LastName))
                        {
                            UserToUpdate.LastName = user.LastName;
                        }
                        else
                        {
                            return StatusCode(403, "The user's Last Name is empty!");
                        }
                        
                        if ((user.RoleId != null || user.RoleId != 0) && RoleID.ToString() == "1")
                        {
                            UserToUpdate.RoleId = user.RoleId;
                        }
                        if ((user.DepartmentId != null || user.DepartmentId != 0) && RoleID.ToString() == "1")
                        {
                            UserToUpdate.DepartmentId = user.DepartmentId;
                        }
                        if ((user.UserAccountStatus != null) && RoleID.ToString() == "1")
                        {
                            UserToUpdate.UserAccountStatus = user.UserAccountStatus;
                        }
                        if ((user.UserApprovalStatus != null || user.UserApprovalStatus != "") && RoleID.ToString() == "1")
                        {
                            UserToUpdate.UserApprovalStatus = user.UserApprovalStatus;
                        }
                        
                        if (!string.IsNullOrEmpty(user.UserEmail))
                        {
                            User checkemail = await _context.Users.Where(ce => ce.UserEmail == user.UserEmail).FirstOrDefaultAsync();
                            if (checkemail != null && (checkemail.UserId != user.UserId)) 
                            {
                                return StatusCode(403, "Email is already in use by another user, kindly use different email!.");
                            }
                            UserToUpdate.UserEmail = user.UserEmail;
                        }
                        else
                        {
                            return StatusCode(403, "The user's Email is empty!");
                        }
                        
                        if (user.UserBiography != null || user.UserBiography != "")
                        {
                            UserToUpdate.UserBiography = user.UserBiography;
                        }
                        if (user.JobTitle != null || user.JobTitle != "")
                        {
                            UserToUpdate.JobTitle = user.JobTitle;
                        }
                        await _context.SaveChangesAsync();
                        return Ok(await _context.Users.Where(ai => ai.UserName == user.UserName).FirstAsync());
                    }
                    else 
                    {
                        return StatusCode(403,"You dont have access to update other user's profile!");
                    }
                }
                else
                {
                    return StatusCode(202, new APIErrReturn { Error = "User does not exists to update!" });
                }
            }
            else
            {
                return StatusCode(400, "Unable to process the request, No Content Body was found!");
            }
        }

        [HttpPost("GetUserProfilePicbyUserID")]
        public async Task<IActionResult> GetUserProfilePicbyUserID([FromBody] UserID ID)
        {
            if (ID != null) 
            {
                var user = await _context.Users.Where(usr => usr.UserId == ID.UID).FirstOrDefaultAsync();
                if (user == null)
                {
                    return StatusCode(204, "No user exist with ID=" + ID.UID);
                }
                else
                {
                    if (user.UserProfilePic == "" || user.UserProfilePic == null)
                    {
                        return StatusCode(204, "No data for profile pic for this user!");
                    }
                    else
                    {
                        return Ok(user.UserProfilePic);
                    }
                }
            }
            else 
            {
                return StatusCode(400, "No proper data is posted!");
            }
           
        }

        [HttpPost("UpdateUserProfilePicbyUserId")]
        public async Task<IActionResult> UpdateUserProfilePicbyUserID([FromBody] UpdateProfilePicById Update) 
        {
            
            if (Update != null) 
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                //var RoleID = identity.FindFirst(ClaimTypes.Role)?.Value;
                var UserID = identity.FindFirst(ClaimTypes.Name)?.Value;

                var user = await _context.Users.Where(usr => usr.UserId == Update.UserID).FirstOrDefaultAsync();
                if (user == null)
                {
                    return StatusCode(204, "No user exist with ID=" + Update.UserID);
                }
                else
                {
                    if (user.UserId.ToString() == UserID)
                    {
                        user.UserProfilePic = Update.ImageUrl;
                        await _context.SaveChangesAsync();
                        return Ok(new { user.UserId, user.FirstName, user.LastName, user.UserProfilePic });

                    }
                    else 
                    {
                        return StatusCode(403,"You does not have access to update other user profile picture!");
                    }
                   
                }
            }
            else 
            {
                return StatusCode(400, "No proper data is posted!");
            }
        }

        [HttpPost("UpdateUserPasswordbyId")]
        public async Task<IActionResult> UpdateUserPasswordbyId([FromBody] UpdateUserPasswordbyId pwd )
        {
            if (pwd != null) 
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                var RoleID = identity.FindFirst(ClaimTypes.Role)?.Value;
                var UserID = identity.FindFirst(ClaimTypes.Name)?.Value;

                User user = await _context.Users.Where(usr => usr.UserId == pwd.UserID).FirstOrDefaultAsync();
                if (user == null)
                {
                    return StatusCode(204, "No user exist with ID=" + pwd.UserID);
                }
                else
                {
                    if (user.UserId.ToString() == UserID || RoleID == "1")
                    {
                        user.UserPassword = _enc.MD5Convert(pwd.NewPassword);
                        await _context.SaveChangesAsync();
                        return Ok(new { user.UserId, user.FirstName, user.LastName, user.UserPassword });
                    }
                    else 
                    {
                        return StatusCode(403, "You does not have access to update other user password!");
                    }
                    
                }

            }
            else 
            {
                return StatusCode(400, "No proper data is posted!");
            }
            
        }

        [Authorize(Roles = ("1"))]
        [HttpPost("UpdateApprovalStatusbyUserID")]
        public async Task<IActionResult> UpdateApprovalStatusbyID([FromBody] UpdateApprovalStatusbyUserId AppStatus)
        {
            if (AppStatus != null)
            {
                var Id = AppStatus.UserID;
                string[] ResultStatus = new string[Id.Length];
                for (int i = 0; i < Id.Length; i++)
                {
                    User usr = await _context.Users.Where(usr => usr.UserId == Id[i]).FirstOrDefaultAsync();
                    if (usr == null)
                    {
                        ResultStatus[i] = "Status update unsucessful!: UserID= " + Id[i] + " Does Not Exist!";
                    }
                    else
                    {
                        usr.UserApprovalStatus = AppStatus.ApprovalStatus;
                        _context.SaveChanges();
                        ResultStatus[i] = "Status update for UserID" + Id[i] + "Successful!";

                    }

                }
                return Ok(ResultStatus);
            }
            else 
            {
                return StatusCode(400, "No proper data is posted!");
            }
            
        }

        [Authorize(Roles = ("1"))]
        [HttpPost("UpdateAccountStatusbyUserId")]
        public async Task<IActionResult> ChangeAccountStatus([FromBody] UpdateAccountStatusbyUserId AccStatus)
        {
            if (AccStatus != null)
            {
                var id = AccStatus.UserID;
                string[] ResultStatus = new string[id.Length];
                for (int i = 0; i < id.Length; i++)
                {
                    User usr = await _context.Users.Where(usr => usr.UserId == id[i]).FirstOrDefaultAsync();
                    if (usr == null)
                    {
                        ResultStatus[i] = "Status update unsucessful!: UserID= " + id[i] + " Does Not Exist!";
                    }
                    else 
                    {

                        usr.UserAccountStatus = AccStatus.AccountStatus;
                        _context.SaveChanges();
                        ResultStatus[i] = "Status update sucessful!: UserID= " + id[i] + " is set to" + usr.UserAccountStatus.ToString() + " !";
                    }
                }

                return Ok(ResultStatus);

            }
            else 
            {
                return StatusCode(400, "No proper data is posted!");
            }
           
        }

        [Authorize(Roles = ("1"))]
        [HttpPost("DeleteUserbyUserID")]
        public async Task<IActionResult> DeleteUserbyID(DeleteUserbyId delete)//change the delete status to true 
        {
            if (delete != null)
            {
                var id = delete.UserID;
                string[] result = new string[delete.UserID.Length];
                for (int d = 0; d < delete.UserID.Length; d++) 
                {
                    var user = await _context.Users.Where(usr => usr.UserId == delete.UserID[d]).FirstOrDefaultAsync();
                    if (user != null)
                    {
                        user.DeleteStatus = delete.DeleteStatus;
                        await _context.SaveChangesAsync();
                        result[d] = "User with ID = " + id[d] +" Delete Status is set to " + delete.DeleteStatus;
                    }
                    else
                    {
                        result[d] = "User with ID = " + id[d] + " not found, no further action has been taken!";
                    }

                }
                return Ok(result);
            }
            else 
            {
                return StatusCode(400, "No proper data is posted!");
            }
        }

        [Authorize(Roles = ("1"))]
        [HttpPost("ChangeRolebyUserID")]
        public async Task<IActionResult> ChangeRole(ChangeRolebyId ChangeRole)//change role
        {
            if (ChangeRole != null)
            {
                var id = ChangeRole.UserID;
                string[] result = new string[ChangeRole.UserID.Length];
                for (int c = 0; c < ChangeRole.UserID.Length; c++)
                {
                    var user = await _context.Users.Where(usr => usr.UserId == ChangeRole.UserID[c]).FirstOrDefaultAsync();
                    if (user != null)
                    {
                        user.RoleId = ChangeRole.RoleID;
                        await _context.SaveChangesAsync();
                        result[c] = "User with ID = " + id[c] + " ,Role ID has been set to " + user.RoleId;
                    }
                    else
                    {
                        result[c] = "User with ID = " + id[c] + " not found, no further action has been taken!";
                    }

                }
                return Ok(result);
            }
            else
            {
                return StatusCode(400, "No proper data is posted!");
            }
        }

        [HttpPost("ProfilePicUploadById")]
        public async Task<IActionResult> ProfilePicUploadById([FromBody] ImageUpload ImgData) 
        {
            if (ImgData != null)
            {
                try
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    //var RoleID = identity.FindFirst(ClaimTypes.Role)?.Value;
                    var UserID = identity.FindFirst(ClaimTypes.Name)?.Value;
                    if (ImgData.UserId.ToString() == UserID)
                    {
                        //Get UserName from UserId
                        var getusername = await _context.Users.Where(x => x.UserId == ImgData.UserId).FirstOrDefaultAsync();
                        var UserName = getusername.UserName;
                        if (ImgData.ImageData == null || ImgData.ImageData == "")
                        {
                            return StatusCode(400, "Image data is null or empty!");
                        }
                        string binpath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                        string newpath = Path.Combine(binpath, binpath + @"\Resources\ProfileImages\" + UserName + ".jpg");
                        if (!Directory.Exists(newpath))
                        {
                            DirectoryInfo Do = Directory.CreateDirectory(Path.Combine(binpath, binpath + @"\Resources\ProfileImages"));
                        }

                        var ImageDataByteArray = Convert.FromBase64String(ImgData.ImageData);
                        var ImageDataStream = new MemoryStream(ImageDataByteArray);
                        ImageDataStream.Position = 0;


                        using (FileStream image = new FileStream(newpath, FileMode.Create, FileAccess.Write))
                        {
                            ImageDataStream.WriteTo(image);
                            image.Close();
                            ImageDataStream.Close();
                        }

                        getusername.UserProfilePic = newpath;
                        await _context.SaveChangesAsync();

                        return StatusCode(200, newpath);

                    }
                    else
                    {
                        return StatusCode(403, "You does not have access to update other user profile picture!");
                    }
                   
                } 
                catch(Exception ex) 
                {
                    return StatusCode(400, ex.ToString());
                }
            }
            

            return StatusCode(400,"No Json Data was posted to API");
        }
    }
}
