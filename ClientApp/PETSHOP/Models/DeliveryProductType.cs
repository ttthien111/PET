using System;
using System.Collections.Generic;

namespace PETSHOP.Models
{
    public partial class DeliveryProductType
    {
        public DeliveryProductType()
        {
            DeliveryProduct = new HashSet<DeliveryProduct>();
        }

        public int DeliveryProductTypeId { get; set; }
        public string DeliveryProductTypeName { get; set; }
        public double? DeliveryProductTypePrice { get; set; }

        public virtual ICollection<DeliveryProduct> DeliveryProduct { get; set; }
    }
}
