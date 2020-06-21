﻿using PETSHOP.Common;
using PETSHOP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PETSHOP.Utils
{
    public static class GetApiDeliveryType
    {
        public static IEnumerable<DeliveryProductType> GetDeliveryProductTypes()
        {
            IEnumerable<DeliveryProductType> types = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.BASE_URI);
                var responseTask = client.GetAsync("deliveryproducttypes");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<DeliveryProductType>>();
                    readTask.Wait();

                    types = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    types = Enumerable.Empty<DeliveryProductType>();
                }
            }

            return types;
        }
    }
}
