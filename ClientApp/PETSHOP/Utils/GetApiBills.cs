using PETSHOP.Areas.Admin.Models;
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
    public static class GetApiBills
    {
        public static IEnumerable<Bill> GetBills(CredentialManage credential)
        {
            IEnumerable<Bill> bills = null;
            using (var client = Common.HelperClient.GetClient(credential.JwToken))
            {
                client.BaseAddress = new Uri(Constants.BASE_URI);
                var responseTask = client.GetAsync(Constants.BILL);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Bill>>();
                    readTask.Wait();

                    bills = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    bills = Enumerable.Empty<Bill>();
                }
            }

            return bills;
        }

        public static void Update(Bill bill, string token)
        {
            using (var client = Common.HelperClient.GetClient(token))
            {
                client.BaseAddress = new Uri(Common.Constants.BASE_URI);

                var putTask = client.PutAsJsonAsync<Bill>(Constants.BILL + "/" + bill.BillId, bill);
                putTask.Wait();
            }
        }
    }
}
