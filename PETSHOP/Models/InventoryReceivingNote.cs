using System;
using System.Collections.Generic;

namespace PETSHOP.Models
{
    public partial class InventoryReceivingNote
    {
        public int InventoryReceivingId { get; set; }
        public DateTime? InventoryReceivingDateReceiving { get; set; }
        public double? InventoryReceivingTotalPrice { get; set; }

        public virtual InventoryReceivingNoteDetailForCostume InventoryReceiving { get; set; }
        public virtual InventoryReceivingNoteDetailForFood InventoryReceivingNavigation { get; set; }
        public virtual InventoryReceivingNoteDetailForToy InventoryReceivingNoteDetailForToy { get; set; }
    }
}
