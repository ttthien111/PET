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
using System.Web.Http;

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

        public static void Update(UserComment comment, string token)
        {
            using (var client = Common.HelperClient.GetClient(token))
            {
                client.BaseAddress = new Uri(Common.Constants.BASE_URI);

                var putTask = client.PutAsJsonAsync<UserComment>(Constants.USER_COMMENT + "/" + comment.UserCommentId, comment);
                putTask.Wait();
            }
        }
    }
}
