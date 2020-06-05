using PETSHOP.Common;
using PETSHOP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PETSHOP.Utils
{
    public static class GetApiUserComments
    {
        public static IEnumerable<UserComment> GetUserComments()
        {
            IEnumerable<UserComment> userComments = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.BASE_URI);
                var responseTask = client.GetAsync("usercomments");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<UserComment>>();
                    readTask.Wait();

                    userComments = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    userComments = Enumerable.Empty<UserComment>();
                }
            }

            return userComments;
        }
    }
}
