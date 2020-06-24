using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PETSHOP.Models.ModelView
{
    public class BillModelView
    {
        public int BillId { get; set; }
        public string BillCode { get; set; }
        public DateTime DateOfPurchase { get; set; }
        public DateTime DateDelivery { get; set; }
        public double TotalPrice { get; set; }
        public int PaymentMethodId { get; set; }
        public string PaymentMethodName { get; set; }
        public List<BillDetailModel> BillDetail { get; set; }
        public int DeliveryProductStateId { get; set; }
        public string DeliveryStateName { get; set; }
        public bool IsDelivery { get; set; }
        public bool IsCancel { get; set; }
        public bool IsApprove { get; set; }
        public bool IsCompleted { get; set; }
        public DeliveryProduct delivery { get; set; }
    }
}
