using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PETSHOP.Areas.Admin.Models;
using PETSHOP.Common;
using PETSHOP.Models.ModelView;

namespace PETSHOP.Areas.Admin.Controllers
{
    [Area(Constants.ADMIN)]
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove(Constants.VM_MANAGE);
            return RedirectToAction("Index", "Home");
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        public IActionResult Login(LoginModel login)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Constants.BASE_URI);

                var postTask = client.PostAsJsonAsync<LoginModel>("LoginAuthentication/AuthenticateManage", login);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    // get credential return
                    var readTask = result.Content.ReadAsAsync<CredentialManage>();
                    readTask.Wait();
                    CredentialManage credential = readTask.Result;

                    // set 1 session for credential
                    HttpContext.Session.SetObject(Constants.VM_MANAGE, credential);

                    if (login.returnUrl != null)
                    {
                        return Redirect(login.returnUrl);
                    }

                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    ViewBag.error = "Tài khoản hoặc mật khẩu không đúng";
                }
            }

            return View("Index");
        }

    }
}