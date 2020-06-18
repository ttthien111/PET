using System;
using System.Collections.Generic;

namespace PETSHOP.Models
{
    public partial class BillDetail
    {
        public int BillDetailId { get; set; }
        public int? BillId { get; set; }
        public int? ProductId { get; set; }
        public double? ProductPriceCurrent { get; set; }
        public int? ProductAmount { get; set; }
        public double? ProductTotalPrice { get; set; }
        public string NoteSize { get; set; }

        public virtual Bill Bill { get; set; }
        public virtual Product Product { get; set; }
    }
}
