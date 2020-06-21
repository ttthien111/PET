using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PETSHOP.Common;
using PETSHOP.Models;
using PETSHOP.Models.ModelView;

namespace PETSHOP.Controllers
{
    public class VerifyController : CheckRegisterVerifyController
    {
        public IActionResult VerifyEmail()
        {
            RegisterModel register = HttpContext.Session.GetObject<RegisterModel>("register");
            return View(register);
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        public IActionResult VerifyEmail(string codeRegister)
        {
            string registerCodeSession = HttpContext.Session.GetString("generateCodeRegister");
            if(codeRegister == registerCodeSession)
            {
                // create a account and user profile
                RegisterModel register = HttpContext.Session.GetObject<RegisterModel>("register");

                // create account
                Account createdAccount = CreateAccount(register);

                // create user profile
                UserProfile createdProfile = CreateProfile(createdAccount, register);

                // Create User Score
                CreateUserScore(createdProfile);

                return RedirectToAction("RegisterStatus");
            }
            else
            {
                return Content("Mã xác minh không đúng");
            }
        }

        private void CreateUserScore(UserProfile createdProfile)
        {
            if (ModelState.IsValid)
            {
                // create account
                UserScore score = new UserScore()
                {
                    UserProfileId = createdProfile.UserProfileId,
                    UserCurrentScore = 0
                };

                // post account
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Common.Constants.BASE_URI);

                    var postTask = client.PostAsJsonAsync<UserScore>("userscores", score);
                    postTask.Wait();
                }
            }
        }

        private UserProfile CreateProfile(Account createdAccount, RegisterModel register)
        {
            UserProfile response = null;
            if (ModelState.IsValid)
            {
                // create account
                UserProfile profile = new UserProfile()
                {
                    UserProfileFirstName = register.FirstName,
                    UserProfileMiddleName = register.MiddleName,
                    UserProfileLastName = register.LastName,
                    UserProfileDob = register.DOB,
                    UserProfilePhoneNumber = register.PhoneNumber,
                    UserProfileEmail = register.Email,
                    UserProfileAvatar = register.Avatar,
                    AccountId = createdAccount.AccountId,
                    CustomerTypeId = 2 // vãng lai
                };

                // post account
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Common.Constants.BASE_URI);

                    var postTask = client.PostAsJsonAsync<UserProfile>("userprofiles", profile);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<UserProfile>();
                        readTask.Wait();

                        response = readTask.Result;
                    }
                }
            }

            return response;
        }

        private Account CreateAccount(RegisterModel register)
        {
            Account response = null;
            if (ModelState.IsValid)
            {
                // create account
                Account account = new Account()
                {
                    AccountRoleId = 3,
                    AccountUserName = register.Email,
                    AccountPassword = Encryptor.MD5Hash(register.Password),
                    IsActive = true,
                    IsLoginExternal = false,
                };

                // post account
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Common.Constants.BASE_URI);

                    var postTask = client.PostAsJsonAsync<Account>("accounts", account);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<Account>();
                        readTask.Wait();

                        response = readTask.Result;
                    }
                }
            }

            return response;
        }

        public IActionResult RegisterStatus(RegisterModel registerModel)
        {
            ViewBag.email = registerModel.Email;

            // delete session
            HttpContext.Session.Remove("generateCodeRegister");
            HttpContext.Session.Remove("register");

            return View();
        }
    }
}