using PETSHOP.Common;
using PETSHOP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PETSHOP.Utils
{
    public static class GetApiFeedbacks
    {
        public static IEnumerable<Feedback> GetFeedbacks(string token)
        {
            IEnumerable<Feedback> res = null;
            using (var client = Common.HelperClient.GetClient(token))
            {
                client.BaseAddress = new Uri(Constants.BASE_URI);
                var responseTask = client.GetAsync(Constants.FEEDBACK);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Feedback>>();
                    readTask.Wait();

                    res = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    res = Enumerable.Empty<Feedback>();
                }
            }

            return res;
        }

        public static void Update(Feedback feedback, string token)
        {
            using (var client = Common.HelperClient.GetClient(token))
            {
                client.BaseAddress = new Uri(Common.Constants.BASE_URI);

                var putTask = client.PutAsJsonAsync<Feedback>(Constants.FEEDBACK + "/" + feedback.FeedbackId, feedback);
                putTask.Wait();
            }
        }
    }
}
