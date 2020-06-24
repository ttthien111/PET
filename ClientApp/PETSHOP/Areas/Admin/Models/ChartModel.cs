using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PETSHOP.Areas.Admin.Models
{
    public class ChartModel
    {
        public  List<Earning7Day> Earning7Days { get; set; }
        public List<Earning7Day> PaymentMethod { get; set; }
        public List<Earning7Day> Bills7Days { get; set; }
    }
}
