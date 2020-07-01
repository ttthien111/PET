using System;
using System.Collections.Generic;

namespace PETSHOP.Models
{
    public partial class InventoryReceivingNote
    {
        public InventoryReceivingNote()
        {
            InventoryReceivingNoteDetailForCostume = new HashSet<InventoryReceivingNoteDetailForCostume>();
            InventoryReceivingNoteDetailForFood = new HashSet<InventoryReceivingNoteDetailForFood>();
            InventoryReceivingNoteDetailForToy = new HashSet<InventoryReceivingNoteDetailForToy>();
        }

        public int InventoryReceivingId { get; set; }
        public DateTime? InventoryReceivingDateReceiving { get; set; }
        public double? InventoryReceivingTotalPrice { get; set; }

        public virtual ICollection<InventoryReceivingNoteDetailForCostume> InventoryReceivingNoteDetailForCostume { get; set; }
        public virtual ICollection<InventoryReceivingNoteDetailForFood> InventoryReceivingNoteDetailForFood { get; set; }
        public virtual ICollection<InventoryReceivingNoteDetailForToy> InventoryReceivingNoteDetailForToy { get; set; }
    }
}
