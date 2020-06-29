using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PETSHOP.Areas.Admin.Models
{
    public class AccountManageDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActivated { get; set; }
        public string AccountRoleName { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public string Address { get; set; }
    }
}
