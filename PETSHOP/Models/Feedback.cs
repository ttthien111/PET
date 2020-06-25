using System;
using System.Collections.Generic;

namespace PETSHOP.Models
{
    public partial class Feedback
    {
        public int FeedbackId { get; set; }
        public string FeedbackContent { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Subject { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool? IsRead { get; set; }
    }
}
