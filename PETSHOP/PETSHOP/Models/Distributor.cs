using System;
using System.Collections.Generic;

namespace PETSHOP.Models
{
    public partial class Distributor
    {
        public Distributor()
        {
            Product = new HashSet<Product>();
        }

        public int DistributorId { get; set; }
        public string DistributorName { get; set; }
        public string DistributorAddress { get; set; }
        public string DistributorPhoneNumber { get; set; }

        public virtual ICollection<Product> Product { get; set; }
    }
}
