using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PETSHOP.Common;
using PETSHOP.Models;
using PETSHOP.Models.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PETSHOP.Utils
{
    public static class GetApiFoodProducts
    {
        public static IEnumerable<FoodProduct> GetFoodProducts()
        {
            IEnumerable<FoodProduct> res = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.BASE_URI);
                var responseTask = client.GetAsync(Constants.FOOD_PRODUCT);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<FoodProduct>>();
                    readTask.Wait();

                    res = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    res = Enumerable.Empty<FoodProduct>();
                }
            }

            return res;
        }
        public static void UpdateStock(FoodProduct stock, string token)
        {
            using (var client = Common.HelperClient.GetClient(token))
            {
                client.BaseAddress = new Uri(Common.Constants.BASE_URI);

                var putTask = client.PutAsJsonAsync<FoodProduct>(Constants.FOOD_PRODUCT + "/" + stock.FoodId, stock);
                putTask.Wait();
            }
        }
    }
}
