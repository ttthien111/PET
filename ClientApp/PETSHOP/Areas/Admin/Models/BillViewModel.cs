using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PETSHOP.Areas.Admin.Models
{
    public class BillViewModel
    {
        public int BillId { get; set; }
        public string UserProfileEmail { get; set; }
        public DateTime DateOfPurchase { get; set; }
        public DateTime? DateOfDelivered { get; set; }
        public double TotalPrice { get; set; }
        public bool IsDelivery { get; set; }
        public string CurrentDeliveryState { get; set; }
        public string PaymentMethodName { get; set; }
        public string GenerateCodeCheck { get; set; }
        public bool IsCancel { get; set; }
        public bool IsApprove { get; set; }
        public bool IsCompleted { get; set; }
    }
}
