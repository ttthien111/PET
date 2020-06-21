using PETSHOP.Common;
using PETSHOP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PETSHOP.Utils
{
    public static class GetApiPaymentMethodTypes
    {
        public static IEnumerable<PaymentMethodType> GetPaymentMethodTypes()
        {
            IEnumerable<PaymentMethodType> res = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.BASE_URI);
                var responseTask = client.GetAsync("paymentmethodtypes");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<PaymentMethodType>>();
                    readTask.Wait();

                    res = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    res = Enumerable.Empty<PaymentMethodType>();
                }
            }

            return res;
        }
    }
}
