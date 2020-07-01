using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PETSHOP.Areas.Admin.Models
{
    public class InventoryReceivingNoteDetailModelView
    {
        public int InventoryReceivingNote_ID { get; set; }
        public DateTime InventoryReceivingNoteDate { get; set; }
        public double TotalPrice { get; set; }
        public List<InventoryRecieveItem> Details { get; set; }
    }
}
