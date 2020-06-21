using PETSHOP.Common;
using PETSHOP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PETSHOP.Utils
{
    public static class GetApiAccounts
    {
        public static IEnumerable<Account> GetAccounts()
        {
            IEnumerable<Account> res = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.BASE_URI);
                var responseTask = client.GetAsync("accounts");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Account>>();
                    readTask.Wait();

                    res = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    res = Enumerable.Empty<Account>();
                }
            }

            return res;
        }

        public static void Update(Account account, string token)
        {
            using (var client = Common.HelperClient.GetClient(token))
            {
                client.BaseAddress = new Uri(Common.Constants.BASE_URI);

                var putTask = client.PutAsJsonAsync<Account>(Constants.ACCOUNT + "/" + account.AccountId, account);
                putTask.Wait();
            }
        }
    }

    
}
