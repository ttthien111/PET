using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PETSHOP.Models.ModelView
{
    public class ProductModelView
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public string ProductImage { get; set; }
        public double ProductPrice { get; set; }
        public double ProductDiscount { get; set; }
        public string ProductDescription { get; set; }
        public int DistributorId { get; set; }
        public bool IsActivated { get; set; }
        public string SlugName { get; set; }
        public DateTime InitAt { get; set; }
        public bool isNew => DateTime.Now.Subtract(InitAt).Days < 10 ? true : false;
        public double Rating { get; set; }
        public int NumberOfPurchases { get; set; }
        public string CatName { get; set; }
    }
}
