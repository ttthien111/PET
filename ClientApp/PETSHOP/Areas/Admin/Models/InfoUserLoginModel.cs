using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PETSHOP.Areas.Admin.Models
{
    public class InfoUserLoginModel
    {
        public string Email { get; set; }
        public string Role { get; set; }
        public DateTime LoginAt { get; set; }
    }
}
