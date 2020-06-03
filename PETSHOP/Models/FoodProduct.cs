using System;
using System.Collections.Generic;

namespace PETSHOP.Models
{
    public partial class FoodProduct
    {
        public int FoodId { get; set; }
        public int? ProductId { get; set; }
        public DateTime? FoodExpiredDate { get; set; }
        public int? FoodAmount { get; set; }

        public virtual Product Product { get; set; }
    }
}
