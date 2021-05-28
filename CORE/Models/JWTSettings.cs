using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CORE.Models
{
    public class JWTSettings
    {
        public string SecretKey { get; set; }
        public double ExpiryDuration { get; set; }
    }
}
