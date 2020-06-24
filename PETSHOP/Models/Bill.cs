using System;
using System.Collections.Generic;

namespace PETSHOP.Models
{
    public partial class Bill
    {
        public Bill()
        {
            BillDetail = new HashSet<BillDetail>();
            DeliveryProduct = new HashSet<DeliveryProduct>();
        }

        public int BillId { get; set; }
        public int UserProfileId { get; set; }
        public DateTime DateOfPurchase { get; set; }
        public DateTime? DateOfDelivered { get; set; }
        public double? TotalPrice { get; set; }
        public bool? IsDelivery { get; set; }
        public int? PaymentMethodTypeId { get; set; }
        public string GenerateCodeCheck { get; set; }
        public bool? IsCancel { get; set; }
        public bool? IsApprove { get; set; }
        public bool? IsCompleted { get; set; }

        public virtual PaymentMethodType PaymentMethodType { get; set; }
        public virtual ICollection<BillDetail> BillDetail { get; set; }
        public virtual ICollection<DeliveryProduct> DeliveryProduct { get; set; }
    }
}
