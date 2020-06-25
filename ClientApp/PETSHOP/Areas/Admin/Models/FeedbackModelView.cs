using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PETSHOP.Areas.Admin.Models
{
    public class FeedbackModelView
    {
        public int FeedbackId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string FeedbackContent { get; set; }
        public string CreatedAt { get; set; }
        public bool IsRead { get; set; }
    }
}
