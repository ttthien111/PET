using PETSHOP.Common;
using PETSHOP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PETSHOP.Utils
{
    public class GetApiBillDetails
    {
        public static IEnumerable<BillDetail> GetBillDetails()
        {
            IEnumerable<BillDetail> billDetails = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.BASE_URI);
                var responseTask = client.GetAsync("billdetails");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<BillDetail>>();
                    readTask.Wait();

                    billDetails = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    billDetails = Enumerable.Empty<BillDetail>();
                }
            }

            return billDetails;
        }
    }
}
