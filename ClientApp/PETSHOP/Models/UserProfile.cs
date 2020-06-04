using System;
using System.Collections.Generic;

namespace PETSHOP.Models
{
    public partial class UserProfile
    {
        public UserProfile()
        {
            UserComment = new HashSet<UserComment>();
            UserScore = new HashSet<UserScore>();
        }

        public int UserProfileId { get; set; }
        public string UserProfileFirstName { get; set; }
        public string UserProfileMiddleName { get; set; }
        public string UserProfileLastName { get; set; }
        public DateTime? UserProfileDob { get; set; }
        public string UserProfileIdentityCard { get; set; }
        public string UserProfilePhoneNumber { get; set; }
        public string UserProfileAvatar { get; set; }
        public string UserProfileAddress { get; set; }
        public string UserProfileEmail { get; set; }
        public int? AccountId { get; set; }
        public int? CustomerTypeId { get; set; }

        public virtual Account Account { get; set; }
        public virtual CustomerType CustomerType { get; set; }
        public virtual ICollection<UserComment> UserComment { get; set; }
        public virtual ICollection<UserScore> UserScore { get; set; }
    }
}
