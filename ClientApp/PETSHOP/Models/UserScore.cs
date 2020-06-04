using System;
using System.Collections.Generic;

namespace PETSHOP.Models
{
    public partial class UserScore
    {
        public int UserScoreId { get; set; }
        public int? UserProfileId { get; set; }
        public int? UserCurrentScore { get; set; }

        public virtual UserProfile UserProfile { get; set; }
    }
}
