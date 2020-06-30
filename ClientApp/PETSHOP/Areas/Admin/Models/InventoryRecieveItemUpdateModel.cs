using PayPal.v1.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PETSHOP.Areas.Admin.Models
{
    public class InventoryRecieveItemUpdateModel
    {
        public int ProductId { get; set; }
        public int SubProductId { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }
        public string Size { get; set; }
    }
}
