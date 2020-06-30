using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PETSHOP.Areas.Admin.Models
{
    public class InventoryRecieveItem
    {
        public int ProductId { get; set; }
        public int SubProductId { get; set; }
        public string ProductName { get; set; }
        public string Size { get; set; }
        public int Amount { get; set; }
        public double Price { get; set; }
        public double Total => Amount * Price;
    }
}
