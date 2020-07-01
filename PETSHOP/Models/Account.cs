using System;
using System.Collections.Generic;

namespace PETSHOP.Models
{
    public partial class Account
    {
        public Account()
        {
            UserProfile = new HashSet<UserProfile>();
        }

        public int AccountId { get; set; }
        public string AccountUserName { get; set; }
        public string AccountPassword { get; set; }
        public bool IsActive { get; set; }
        public int AccountRoleId { get; set; }
        public string Jwtoken { get; set; }
        public bool? IsLoginExternal { get; set; }

        public virtual AccountRole AccountRole { get; set; }
        public virtual ICollection<UserProfile> UserProfile { get; set; }
    }
}
