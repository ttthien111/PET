using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PETSHOP.Models.ModelView
{
    public class BillDetailModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Amount { get; set; }
        public double Price { get; set; }
        public double TotalPrice => Amount * Price;
        public string NoteSize { get; set; }
        public string Image { get; set; }
    }
}
