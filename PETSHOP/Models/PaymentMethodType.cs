using System;
using System.Collections.Generic;

namespace PETSHOP.Models
{
    public partial class PaymentMethodType
    {
        public PaymentMethodType()
        {
            Bill = new HashSet<Bill>();
        }

        public int PaymentMethodTypeId { get; set; }
        public string PaymentMethodTypeName { get; set; }

        public virtual ICollection<Bill> Bill { get; set; }
    }
}
