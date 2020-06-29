using PETSHOP.Common;
using PETSHOP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PETSHOP.Utils
{
    public class GetApiAccountManage
    {
        public static IEnumerable<AccountManage> GetAccountManages(string token)
        {
            IEnumerable<AccountManage> res = null;
            using (var client = HelperClient.GetClient(token))
            {
                client.BaseAddress = new Uri(Constants.BASE_URI);
                var responseTask = client.GetAsync(Constants.ACCOUNT_MANAGE);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<AccountManage>>();
                    readTask.Wait();

                    res = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    res = Enumerable.Empty<AccountManage>();
                }
            }

            return res;
        }

    }
}
