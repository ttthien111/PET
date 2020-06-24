using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;
using PETSHOP.Common;
using PETSHOP.Models;
namespace PETSHOP.Utils
{
    public static class GetApiProducts
    {
        public static IEnumerable<Product> GetProducts()
        {
            IEnumerable<Product> products = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.BASE_URI);
                var responseTask = client.GetAsync(Constants.PRODUCT);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Product>>();
                    readTask.Wait();

                    products = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..
                    products = Enumerable.Empty<Product>();
                }
            }

            return products;
        }
    }
}
