using PETSHOP.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PETSHOP.Areas.Admin.Models
{
    public class CredentialManage
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public string Address { get; set; }
        public string JwToken { get; set; }
        public int AccountRoleId { get; set; }
        public string AccountRoleName => GetApiAccountRoles.GetAccountRoles().SingleOrDefault(p => p.AccountRoleId == AccountRoleId).AccountRoleName;
        public bool isActivated { get; set; }
    }
}
