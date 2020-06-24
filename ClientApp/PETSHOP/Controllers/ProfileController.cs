using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ASPCore_Final.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PETSHOP.Common;
using PETSHOP.Models;
using PETSHOP.Models.ModelView;
using PETSHOP.Utils;

namespace PETSHOP.Controllers
{
    public class ProfileController : CheckAuthenController
    {
        public IActionResult Index()
        {
            CredentialModel credential = JsonConvert.DeserializeObject<CredentialModel>(HttpContext.Session.GetString("vm"));
            UserProfile profile = credential.Profile;
            return View(profile);
        }

        [HttpPost]
        public IActionResult EditProfile(UserProfile profile, IFormFile avatarFile)
        {
            if (ModelState.IsValid)
            {
                //get user current
                UserProfile user = GetApiUserProfile.GetUserProfiles().SingleOrDefault(p => p.UserProfileEmail == profile.UserProfileEmail);
                profile.AccountId = user.AccountId;
                profile.CustomerTypeId = user.CustomerTypeId;
                profile.UserProfileAvatar = user.UserProfileAvatar;

                if (avatarFile != null)
                {
                    string extension = Path.GetExtension(avatarFile.FileName);

                    if (SlugHelper.CheckExtension(extension))
                    {
                        // delete img current
                        var pathCurrent = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\avatar", profile.UserProfileAvatar);

                        if (System.IO.File.Exists(pathCurrent))
                        {
                            System.IO.File.Delete(pathCurrent);
                        }

                        string avatarName = Encryptor.RandomString(12);

                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images/avatar", avatarName + extension);

                        using (var file = new FileStream(path, FileMode.Create))
                        {
                            avatarFile.CopyTo(file);
                        }
                        profile.UserProfileAvatar = avatarName + extension;
                    }
                    else
                    {
                        ViewBag.error = "Kiểu file không hỗ trợ (jpg, png, ...)";
                        return View("Index", profile);
                    }
                }

                CredentialModel credential = JsonConvert.DeserializeObject<CredentialModel>(HttpContext.Session.GetString("vm"));

                UserProfile update = UpdateProfile(profile);
                if(update != null)
                {
                    // update sessionlogin
                    credential.Profile = update;

                    HttpContext.Session.SetObject("vm", credential);
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Cập nhật thất bại";
                }
            }
            else
            {
                ViewBag.error = "Dữ liệu không thể để trống";
            }

            return View("Index", profile);
        }



        private UserProfile UpdateProfile(UserProfile profile)
        {
            CredentialModel credential = JsonConvert.DeserializeObject<CredentialModel>(HttpContext.Session.GetString("vm"));
            string token = credential.JwToken;

            using (HttpClient client = Common.HelperClient.GetClient(token))
            {
                client.BaseAddress = new Uri(Common.Constants.BASE_URI);

                var postTask = client.PutAsJsonAsync<UserProfile>("userprofiles/"+ profile.UserProfileId, profile);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<UserProfile>();
                    readTask.Wait();

                    return readTask.Result;
                }
                else
                {
                    return null;
                }
            }
        }

        public IActionResult GenerateVerifyCode()
        {
            VerifyModel verifyModel = new VerifyModel()
            {
                CodeVerify = Encryptor.RandomString(6),
                Status = false
            };

            HttpContext.Session.SetObject("verifyChangePW", verifyModel);
            // send code

            // get credential
            CredentialModel credential = JsonConvert.DeserializeObject<CredentialModel>(HttpContext.Session.GetString("vm"));
            SenderEmail.SendMail(credential.Profile.UserProfileEmail, "PETSHOP VERIFY CHANGE PASSWORD", "Code verify: " + verifyModel.CodeVerify);
            return NoContent();
        }

        public IActionResult VerifyChangePassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult VerifyChangePassword(string codeVerify)
        {
            // get code verify session
            VerifyModel verifyModel = HttpContext.Session.GetObject<VerifyModel>("verifyChangePW");

            if ( verifyModel!= null && codeVerify == verifyModel.CodeVerify)
            {
                verifyModel.Status = true;

                HttpContext.Session.SetObject("verifyChangePW", verifyModel);
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
            VerifyModel verifyModel = HttpContext.Session.GetObject<VerifyModel>("verifyChangePW");
            if (verifyModel.Status)
            {
                // return change pasword page
                return View();
            }
            else
            {
                // return to verify page
                return RedirectToAction("VerifyChangePassword");
            }
        }

        [HttpPost]
        public IActionResult ChangePassword(string newPassword)
        {
            VerifyModel verifyModel = HttpContext.Session.GetObject<VerifyModel>("verifyChangePW");

            CredentialModel credential = JsonConvert.DeserializeObject<CredentialModel>(HttpContext.Session.GetString("vm"));
            string token = credential.JwToken;
            // get account for update
            Account account = GetApiAccounts.GetAccounts().SingleOrDefault(p => p.AccountId == Int32.Parse(credential.AccountId));

            // update password
            account.AccountPassword = Encryptor.MD5Hash(newPassword);

            GetApiAccounts.Update(account, token);

            return NoContent();
        }
    }
}