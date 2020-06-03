using System;
using System.Collections.Generic;

namespace PETSHOP.Models
{
    public partial class ToyProduct
    {
        public int ToyId { get; set; }
        public int? ProductId { get; set; }
        public int? ToyAmount { get; set; }

        public virtual Product Product { get; set; }
    }
}
