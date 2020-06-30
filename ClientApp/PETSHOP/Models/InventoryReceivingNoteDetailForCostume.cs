using System;
using System.Collections.Generic;

namespace PETSHOP.Models
{
    public partial class InventoryReceivingNoteDetailForCostume
    {
        public int InventoryReceivingId { get; set; }
        public int CostumeProductId { get; set; }
        public string CostumeProductSize { get; set; }
        public int CostumeProductAmount { get; set; }
    }
}
