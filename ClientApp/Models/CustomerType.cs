using System;
using System.Collections.Generic;

namespace PETSHOP.Models
{
    public partial class CustomerType
    {
        public CustomerType()
        {
            UserProfile = new HashSet<UserProfile>();
        }

        public int CustomerTypeId { get; set; }
        public string CustomerTypeName { get; set; }
        public int? CustomerTypeScore { get; set; }

        public virtual ICollection<UserProfile> UserProfile { get; set; }
    }
}
