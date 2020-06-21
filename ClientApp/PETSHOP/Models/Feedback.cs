using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PETSHOP.Models
{
    public class Feedback
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string FeedbackContent { get; set; }
        public string CreatedAt { get; set; }

    }
}
