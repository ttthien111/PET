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
    public static class GetApiMyBills
    {
        public static IEnumerable<Bill> GetBills(CredentialModel credential)
        {
            IEnumerable<Bill> bills = null;
            using (var client = Common.HelperClient.GetClient(credential.JwToken))
            {
                client.BaseAddress = new Uri(Constants.BASE_URI);
                var responseTask = client.GetAsync(Constants.MY_BILL + "/" + credential.Profile.UserProfileId);
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
    }
}
