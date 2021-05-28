using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;
using Microsoft.Extensions.Options;

namespace CORE.Encryption
{
    public class MD5encrypt
    {
        private readonly EncryptSalt _salt;
        public MD5encrypt(EncryptSalt salt) 
        {
            _salt = salt;
        }
        public string MD5Convert(string password) 
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            string SaltedPwd = password + _salt.Salt;
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(SaltedPwd));
            byte[] bytes = md5.Hash;
            string result = BitConverter.ToString(bytes).Replace("-", String.Empty);
            return result.ToLower();
            
        }
    }
}
