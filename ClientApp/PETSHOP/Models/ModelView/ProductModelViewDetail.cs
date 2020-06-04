using System;
using System.Collections.Generic;

namespace PETSHOP.Models.ModelView
{
   public class ProductModelViewDetail
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public string ProductImage { get; set; }
        public double ProductPrice { get; set; }
        public double ProductDiscount { get; set; }
        public string ProductDescription { get; set; }
        public string DistributorName { get; set; }
        public bool IsActivated { get; set; }
        public string SlugName { get; set; }
        public DateTime InitAt { get; set; }
        public double Rating { get; set; }
        public DateTime FoodExpiredDate { get; set; }
        public List<CostumeSizeModel> CostumeSize { get; set; }
        public int? Amount { get; set; }
    }
}
