using System;
using System.Collections.Generic;

namespace PETSHOP.Models
{
    public partial class DeliveryProductState
    {
        public DeliveryProductState()
        {
            DeliveryProduct = new HashSet<DeliveryProduct>();
        }

        public int DeliveryProductStateId { get; set; }
        public string DeliveryProductStateName { get; set; }

        public virtual ICollection<DeliveryProduct> DeliveryProduct { get; set; }
    }
}
