using System;
using System.Collections.Generic;

namespace PETSHOP.Models
{
    public partial class UserComment
    {
        public int UserCommentId { get; set; }
        public int UserProfileId { get; set; }
        public int ProductId { get; set; }
        public int? Score { get; set; }
        public string UserCommentContent { get; set; }
        public DateTime? UserCommentPostedDate { get; set; }
        public bool? UserCommentApproved { get; set; }

        public virtual Product Product { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }
}
