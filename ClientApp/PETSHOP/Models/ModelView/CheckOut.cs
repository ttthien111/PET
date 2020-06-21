using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PETSHOP.Models.ModelView
{
    public class CheckOut
    {
        public int UserProfileId { get; set; }
        public DateTime DateOfPurchase { get; set; }
        public DateTime DateOfDelivered { get; set; }
        public double TotalPrice { get; set; }
        public bool IsDelivery { get; set; }
        public int PaymentMethodTypeId { get; set; }
        public string DeliveryProductAddress { get; set; }
        public string DeliveryProductPhoneNumber { get; set; }
        public string DeliveryProductNote { get; set; }
        public int DeliveryProductStateId { get; set; }
        public int? DeliveryProductTypeId { get; set; }
    }
}
