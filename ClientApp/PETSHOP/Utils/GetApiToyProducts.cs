using PETSHOP.Common;
using PETSHOP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PETSHOP.Utils
{
    public static class GetApiToyProducts
    {
        public static IEnumerable<ToyProduct> GetToyProducts()
        {
            IEnumerable<ToyProduct> res = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.BASE_URI);
                var responseTask = client.GetAsync(Constants.TOY_PRODUCT);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<ToyProduct>>();
                    readTask.Wait();

                    res = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    res = Enumerable.Empty<ToyProduct>();
                }
            }

            return res;
        }

        public static void UpdateStock(ToyProduct stock, string token)
        {
            using (var client = Common.HelperClient.GetClient(token))
            {
                client.BaseAddress = new Uri(Common.Constants.BASE_URI);

                var putTask = client.PutAsJsonAsync<ToyProduct>(Constants.TOY_PRODUCT + "/" + stock.ToyId, stock);
                putTask.Wait();
            }
        }
    }
}
