using System;
using System.Collections.Generic;

namespace PETSHOP.Models
{
    public partial class DeliveryProduct
    {
        public int DeliveryProductId { get; set; }
        public int? DeliveryProductBillId { get; set; }
        public string DeliveryProductAddress { get; set; }
        public string DeliveryProductPhoneNumber { get; set; }
        public string DeliveryProductNote { get; set; }
        public int? DeliveryProductStateId { get; set; }
        public int? DeliveryProductTypeId { get; set; }

        public virtual Bill DeliveryProductBill { get; set; }
        public virtual DeliveryProductState DeliveryProductState { get; set; }
        public virtual DeliveryProductType DeliveryProductType { get; set; }
    }
}
