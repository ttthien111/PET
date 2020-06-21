using PETSHOP.Common;
using PETSHOP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PETSHOP.Utils
{
    public static class GetApiDistributors
    {
        public static IEnumerable<Distributor> GetDistributors()
        {
            IEnumerable<Distributor> distributors = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.BASE_URI);
                var responseTask = client.GetAsync("distributors");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Distributor>>();
                    readTask.Wait();

                    distributors = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    distributors = Enumerable.Empty<Distributor>();
                }
            }

            return distributors;
        }
    }
}
