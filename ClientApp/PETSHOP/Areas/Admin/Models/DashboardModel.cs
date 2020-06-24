using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PETSHOP.Areas.Admin.Models
{
    public class DashboardModel
    {

        public DashboardModel()
        {
            EarningDay = 0;
            EarningMonth = 0;
            BillsDay = 0;
            BillsMonth = 0;
        }
        public double EarningDay { get; set; }
        public double EarningMonth { get; set; }
        public int BillsMonth { get; set; }
        public int BillsDay { get; set; }

    }
}
