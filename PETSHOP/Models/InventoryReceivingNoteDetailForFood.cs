using System;
using System.Collections.Generic;

namespace PETSHOP.Models
{
    public partial class InventoryReceivingNoteDetailForFood
    {
        public int InventoryReceivingId { get; set; }
        public int FoodProductId { get; set; }
        public int? FoodProductAmount { get; set; }
        public double? FoodProductPrice { get; set; }

        public virtual InventoryReceivingNote InventoryReceiving { get; set; }
    }
}
