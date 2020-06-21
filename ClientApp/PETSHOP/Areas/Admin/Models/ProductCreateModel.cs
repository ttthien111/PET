using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PETSHOP.Areas.Admin.Models
{
    public class ProductCreateModel
    {
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public string ProductImage { get; set; }
        public double ProductPrice { get; set; }
        public double ProductDiscount { get; set; }
        public string ProductDescription { get; set; }
        public int DistributorId { get; set; }

    }
}
