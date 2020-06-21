using PETSHOP.Common;
using PETSHOP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PETSHOP.Utils
{
    public static class GetApiUserProfile
    {
        public static IEnumerable<UserProfile> GetUserProfiles()
        {
            IEnumerable<UserProfile> userProfiles = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.BASE_URI);
                var responseTask = client.GetAsync("userprofiles");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<UserProfile>>();
                    readTask.Wait();

                    userProfiles = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    userProfiles = Enumerable.Empty<UserProfile>();
                }
            }

            return userProfiles;
        }
    }
}
