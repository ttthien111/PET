using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PETSHOP.Models.ModelView
{
    public class CommentModel
    {
        public string UserProfileId { get; set; }
        public string ProductId { get; set; }
        public string Score { get; set; }
        public string UserCommentContent { get; set; }
        public string UserCommentPostedDate { get; set; }
        public bool UserCommentApproved { get; set; }
    }
}
