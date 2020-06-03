using System;
using System.Collections.Generic;

namespace PETSHOP.Models
{
    public partial class CostumeProduct
    {
        public int CostumeId { get; set; }
        public int? ProductId { get; set; }
        public string CostumeSize { get; set; }
        public int? CostumeAmountSize { get; set; }

        public virtual Product Product { get; set; }
    }
}
