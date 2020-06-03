using System;
using System.Collections.Generic;

namespace PETSHOP.Models
{
    public partial class InventoryReceivingNoteDetailForFood
    {
        public int InventoryReceivingId { get; set; }
        public int? FoodProductId { get; set; }
        public int? FoodProductAmount { get; set; }

        public virtual InventoryReceivingNote InventoryReceivingNote { get; set; }
    }
}
