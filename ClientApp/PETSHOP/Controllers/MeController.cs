using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PETSHOP.Models.ModelView;
using PETSHOP.Models;
using System.Net.Http;
using PETSHOP.Utils;

namespace PETSHOP.Controllers
{
    public class MeController : Controller
    {
        public IActionResult Contact()
        {
            if (HttpContext.Session.GetString("vm") != null)
            {
                CredentialModel credential = JsonConvert.DeserializeObject<CredentialModel>(HttpContext.Session.GetString("vm"));
                ViewBag.loginInfo = credential != null ? credential : null;
            }

            // get list feedbacks 
            return View();
        }

        [HttpPost]
        public IActionResult AddFeedback(string feedback)
        {
            if (ModelState.IsValid)
            {
                Feedback fb = JsonConvert.DeserializeObject<Feedback>(feedback);
                CredentialModel credential = JsonConvert.DeserializeObject<CredentialModel>(HttpContext.Session.GetString("vm"));
                string token = credential.JwToken;

                // post comment
                using (HttpClient client = Common.HelperClient.GetClient(token))
                {
                    client.BaseAddress = new Uri(Common.Constants.BASE_URI);

                    var postTask = client.PostAsJsonAsync<Feedback>("feedbacks", fb);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return NoContent();
                    }
                }
            }
            return NoContent();
        }
    }
}