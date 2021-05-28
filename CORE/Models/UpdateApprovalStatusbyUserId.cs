using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CORE.Models
{
    public class UpdateApprovalStatusbyUserId
    {
        public int[] UserID { get; set; }
        public string ApprovalStatus { get; set; }
    }
}
