using System;
using System.Collections.Generic;

namespace PETSHOP.Models
{
    public partial class AccountRole
    {
        public AccountRole()
        {
            Account = new HashSet<Account>();
            AccountManage = new HashSet<AccountManage>();
        }

        public int AccountRoleId { get; set; }
        public string AccountRoleName { get; set; }
        public bool? AccountRoleInsert { get; set; }
        public bool? AccountRoleDelete { get; set; }
        public bool? AccountRoleUpdate { get; set; }
        public bool? AccountRoleReadApi { get; set; }

        public virtual ICollection<Account> Account { get; set; }
        public virtual ICollection<AccountManage> AccountManage { get; set; }
    }
}
