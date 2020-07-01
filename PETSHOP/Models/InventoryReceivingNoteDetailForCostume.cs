using System;
using System.Collections.Generic;

namespace PETSHOP.Models
{
    public partial class InventoryReceivingNoteDetailForCostume
    {
        public int InventoryReceivingId { get; set; }
        public int CostumeProductId { get; set; }
        public string CostumeProductSize { get; set; }
        public int? CostumeProductAmount { get; set; }
        public double? CostumeProductPrice { get; set; }

        public virtual InventoryReceivingNote InventoryReceiving { get; set; }
    }
}
