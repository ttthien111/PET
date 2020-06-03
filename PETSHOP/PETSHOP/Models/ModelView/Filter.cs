using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PETSHOP.Models.ModelView
{
    public class Filter
    {
        public double PriceTo { get; set; }
        public double PriceFrom { get; set; }
        public bool isNew { get; set; }
        public bool isDiscount { get; set; }
        public string ratings { get; set; }
        public bool sort { get; set; }
    }
}
