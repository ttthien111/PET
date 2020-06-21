using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PETSHOP.Models.ModelView
{
    public class InvoiceModel
    {
        public string DeliveryProductStateName { get; set; }
        public string SlugState { get; set; }
        public List<BillModelView> bills { get; set; }
    }
}
