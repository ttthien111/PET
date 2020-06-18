using System;
using System.Collections.Generic;

namespace PETSHOP.Models
{
    public partial class Product
    {
        public Product()
        {
            BillDetail = new HashSet<BillDetail>();
            CostumeProduct = new HashSet<CostumeProduct>();
            FoodProduct = new HashSet<FoodProduct>();
            ToyProduct = new HashSet<ToyProduct>();
            UserComment = new HashSet<UserComment>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public string ProductImage { get; set; }
        public double ProductPrice { get; set; }
        public double ProductDiscount { get; set; }
        public string ProductDescription { get; set; }
        public int DistributorId { get; set; }
        public bool? IsActivated { get; set; }
        public string SlugName { get; set; }
        public DateTime? InitAt { get; set; }
        public int? NumberOfPurchases { get; set; }

        public virtual Category Category { get; set; }
        public virtual Distributor Distributor { get; set; }
        public virtual ICollection<BillDetail> BillDetail { get; set; }
        public virtual ICollection<CostumeProduct> CostumeProduct { get; set; }
        public virtual ICollection<FoodProduct> FoodProduct { get; set; }
        public virtual ICollection<ToyProduct> ToyProduct { get; set; }
        public virtual ICollection<UserComment> UserComment { get; set; }
    }
}
