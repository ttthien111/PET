using System;
using System.Collections.Generic;

namespace PETSHOP.Models
{
    public partial class InventoryReceivingNoteDetailForToy
    {
        public int InventoryReceivingId { get; set; }
        public int ToyProductId { get; set; }
        public int? ToyProductAmount { get; set; }
        public double? ToyProductPrice { get; set; }

        public virtual InventoryReceivingNote InventoryReceiving { get; set; }
    }
}
