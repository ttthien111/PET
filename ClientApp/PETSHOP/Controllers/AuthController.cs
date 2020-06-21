using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PETSHOP.Common;
using PETSHOP.Models;
using PETSHOP.Models.ModelView;
using PETSHOP.Utils;

namespace PETSHOP.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("vm");
            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            LoginModel login = new LoginModel();
            ViewBag.returnUrl = Request.Headers["Referer"].ToString();
            return View(login);
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        public IActionResult Login(LoginModel login)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(Common.Constants.BASE_URI);

                var postTask = client.PostAsJsonAsync<LoginModel>("LoginAuthentication/Authenticate", login);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    // get credential return
                    var readTask = result.Content.ReadAsAsync<CredentialModel>();
                    readTask.Wait();
                    CredentialModel credential = readTask.Result;

                    // get user profile
                    UserProfile profile = GetApiUserProfile.GetUserProfiles().SingleOrDefault(p => p.AccountId == Convert.ToInt32(credential.AccountId));
                    credential.Profile = profile;

                    // set 1 session for credential
                    HttpContext.Session.SetObject("vm", credential);

                    if(login.returnUrl != null)
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

            return View();
        }


        public IActionResult LoginExternal(string loginEx)
        {
            Account createdAccount = null;
            UserProfile userProfile = null;

            LoginEx login = JsonConvert.DeserializeObject<LoginEx>(loginEx);

            if(login.Email == null)
            {
                login.Email = "customer_" + Encryptor.RandomString(6) + "@petshop.com"; 
            }

            UserProfile profile = GetApiUserProfile.GetUserProfiles().SingleOrDefault(p => p.UserProfileEmail == login.Email);
            // create if null
            if(profile == null)
            {
                string passwordTemp = Encryptor.RandomString(12);
                // create account
                RegisterModel register = new RegisterModel()
                {
                    Email = login.Email,
                    FirstName = login.FirstName,
                    MiddleName = login.MiddleName,
                    LastName = login.LastName,
                    Password = passwordTemp,
                    IsLoginExternal = true,
                    DOB = "1990/1/1",
                    Avatar = "noimage.png"
                };

                createdAccount = CreateAccount(register);

                // create profile
                userProfile = CreateProfile(createdAccount, register);
                // create user score
                CreateUserScore(userProfile);
            }


            // request token login
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.BASE_URI);

                var postTask = client.PostAsJsonAsync<LoginEx>("LoginAuthentication/AuthenticateExternal", login);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<CredentialModel>();
                    readTask.Wait();

                    CredentialModel response = readTask.Result;

                    // get user profile
                    UserProfile res_profile = GetApiUserProfile.GetUserProfiles().SingleOrDefault(p => p.AccountId == Convert.ToInt32(response.AccountId));
                    response.Profile = profile;

                    // set 1 session for credential
                    HttpContext.Session.SetObject("vm", response);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View();
                }
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
                    IsLoginExternal = register.IsLoginExternal
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

        public IActionResult Register()
        {
            RegisterModel register = new RegisterModel();
            return View(register);
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        public IActionResult Register(RegisterModel register, IFormFile avatarFile)
        {
            if (ModelState.IsValid)
            {
                if (!ExistEmail(register.Email))
                {
                    // create session register code
                    string generateCodeRegister = Encryptor.RandomString(12);
                    HttpContext.Session.SetString("generateCodeRegister", generateCodeRegister);

                    // create session register model 
                    if (avatarFile != null)
                    {
                        string extension = Path.GetExtension(avatarFile.FileName);  
                        
                        if (CheckExtension(extension))
                        {
                            string avatarName = Encryptor.RandomString(12);

                            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images/avatar", avatarName + extension);
                            using (var file = new FileStream(path, FileMode.Create))
                            {
                                avatarFile.CopyTo(file);
                            }
                            register.Avatar = avatarName + extension;
                        }
                        else
                        {
                            ModelState.AddModelError("", "Kiểu file không hỗ trợ (jpg, png, ...)");
                        }
                    }

                    // send coderegister to email
                    SenderEmail.SendMail(register.Email, "VERIFY", "Code verify: " + generateCodeRegister);

                    HttpContext.Session.SetObject("register", register);
                    return RedirectToAction("VerifyEmail","Verify");
                }
                else
                {
                    ModelState.AddModelError("", "Email đã tồn tại");
                }
            }
            else
            {
                ModelState.AddModelError("", "Email không hợp lệ");
            }

            return View();
        }

        private bool CheckExtension(string extension)
        {
            string[] allowedExtensions = new[] { ".jpg", ".png" };
            return allowedExtensions.Contains(extension);
        }

        public bool ExistEmail(string email)
        {
            return GetApiUserProfile.GetUserProfiles().SingleOrDefault(p => p.UserProfileEmail == email) != null;
        }

        public IActionResult ExistEmailValidate(string email)
        {
            if (!email.Contains("@"))
            {
                return Content("exception-Email không khả dụng");
            }
            else
            {
                if (GetApiUserProfile.GetUserProfiles().SingleOrDefault(p => p.UserProfileEmail == email) != null)
                {
                    return Content("exist-Email đã tồn tại");
                }
                else
                {
                    return Content("available-Email khả dụng");
                }
            }
            
        }

        public IActionResult GenerateVerifyCode(string email)
        {
            VerifyModel verifyModel = new VerifyModel()
            {
                CodeVerify = Encryptor.RandomString(6),
                Status = false,
                Email = email
            };

            HttpContext.Session.SetObject("verifyForgetPW", verifyModel);
            // send code

            SenderEmail.SendMail(email, "PETSHOP VERIFY RESTORE ACCOUNT", "Code verify: " + verifyModel.CodeVerify);
            return NoContent();
            throw new Exception();
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        public IActionResult ForgetPassword(string codeVerify)
        {
            // get code verify session
            VerifyModel verifyModel = HttpContext.Session.GetObject<VerifyModel>("verifyForgetPW");

            if (verifyModel != null && codeVerify == verifyModel.CodeVerify)
            {
                verifyModel.Status = true;

                HttpContext.Session.SetObject("verifyForgetPW", verifyModel);
                return RedirectToAction("ChangePassword");
            }
            else
            {
                ViewBag.error = "Mã xác minh không đúng !";
                return View();
            }
        }

        public IActionResult ChangePassword()
        {
            VerifyModel verifyModel = HttpContext.Session.GetObject<VerifyModel>("verifyForgetPW");
            if (verifyModel == null)
                return RedirectToAction("Index", "Home");
            if (verifyModel.Status)
            {
                // return change pasword page
                return View();
            }
            else
            {
                // return to verify page
                return RedirectToAction("ForgetPassword");
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        public IActionResult ChangePassword(string password)
        {
            VerifyModel verifyModel = HttpContext.Session.GetObject<VerifyModel>("verifyForgetPW");
            //get string token from server

            // get account for update
            Account account = GetApiAccounts.GetAccounts().SingleOrDefault(p => p.AccountUserName == verifyModel.Email);

            // update password
            account.AccountPassword = Encryptor.MD5Hash(password);

            GetApiAccounts.Update(account, null);

            HttpContext.Session.Remove("verifyForgetPW");
            return RedirectToAction("Login", "Auth");
        }

        public IActionResult CheckEmailExist(string email)
        {
            if(GetApiAccounts.GetAccounts().SingleOrDefault(p=>p.AccountUserName == email) != null)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}