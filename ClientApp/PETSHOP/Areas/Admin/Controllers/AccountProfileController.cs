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
using PETSHOP.Areas.Admin.Models;
using PETSHOP.Common;
using PETSHOP.Models;
using PETSHOP.Utils;

namespace PETSHOP.Areas.Admin.Controllers
{
    [Area(Constants.ADMIN)]
    public class AccountProfileController : CheckAuthenticateManageController
    {

        public IActionResult UpdateProfile(string email)
        {
            CredentialManage credential = JsonConvert.DeserializeObject<CredentialManage>(HttpContext.Session.GetString(Constants.VM_MANAGE));
            AccountManage profile = GetApiAccountManage.GetAccountManages(credential.JwToken)
                                                .Select(p => new AccountManage()
                                                {
                                                    Email = p.Email,
                                                    AccountRoleId = p.AccountRoleId,
                                                    FullName = p.FullName,
                                                    IsActivated = p.IsActivated,
                                                    Avatar = p.Avatar,
                                                    Address = p.Address

                                                }).SingleOrDefault(p => p.Email == email);
            ViewBag.AccountRoleName = GetApiAccountRoles.GetAccountRoles().SingleOrDefault(k => k.AccountRoleId == profile.AccountRoleId).AccountRoleName;
            ViewBag.Email = profile.Email;
            ViewBag.FullName = profile.FullName;
            ViewBag.DiaChi = profile.Address;
            return View();
        }
        [HttpPost]
        public IActionResult UpdateProfile(string email, AccountManage profileInput, IFormFile Avatar)
        {

            CredentialManage credential = JsonConvert.DeserializeObject<CredentialManage>(HttpContext.Session.GetString(Constants.VM_MANAGE));
            string token = credential.JwToken;
            AccountManage profile = GetApiAccountManage.GetAccountManages(credential.JwToken)
                                                .Select(p => new AccountManage()
                                                {
                                                    Email = p.Email,
                                                    AccountRoleId = p.AccountRoleId,
                                                    FullName = profileInput.FullName,
                                                    IsActivated = p.IsActivated,
                                                    Avatar = p.Avatar,
                                                    Address = profileInput.Address,
                                                    Password = p.Password
                                                }).SingleOrDefault(p => p.Email == email);

            string accountImg = Encryptor.RandomString(12);
            string extension = Avatar != null ? Path.GetExtension(Avatar.FileName) : "";
            if (Avatar != null)
            {
                if (SlugHelper.CheckExtension(extension))
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images/avatar", accountImg + extension);
                    using (var file = new FileStream(path, FileMode.Create))
                    {
                        Avatar.CopyTo(file);
                    }
                    profile.Avatar = accountImg + extension;
                }
                else
                {
                    ModelState.AddModelError("", Constants.EXTENSION_IMG_NOT_SUPPORT);
                    return Content(Constants.EXTENSION_IMG_NOT_SUPPORT);
                }
            }

            using (HttpClient client = HelperClient.GetClient(token))
            {
                client.BaseAddress = new Uri(Constants.BASE_URI);

                var putTask = client.PutAsJsonAsync<AccountManage>(Constants.ACCOUNT_MANAGE +"/"+ profile.Email, profile);
                putTask.Wait();

                var result = putTask.Result;
                return View();
            }
        }

        public IActionResult VerifyPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult VerifyPassword(AccountProfilePassword profile)
        {
            CredentialManage credential = JsonConvert.DeserializeObject<CredentialManage>(HttpContext.Session.GetString(Constants.VM_MANAGE));
            profile.Email = credential.Email;
            if (GetApiAccountManage.GetAccountManages(credential.JwToken).Any(p=>p.Email == profile.Email && p.Password == Encryptor.MD5Hash(profile.Password)))
            {
                return RedirectToAction("ChangePassword");
            }
            ViewBag.result = "Sai thông tin đăng nhập";
            return View();
        }


        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(string pass1, string pass2)
        {
            if (pass1 != pass2)
            {
                ViewBag.result += @"Mật khẩu không khớp <br>";
            }
            if (pass1.Length < 6)
            {
                ViewBag.result += @"Mật khẩu chưa đủ mạnh";
            }
            if (pass1.Length >= 6 && pass2.Length >=6 && pass1 == pass2)
            {
                CredentialManage credential = JsonConvert.DeserializeObject<CredentialManage>(HttpContext.Session.GetString(Constants.VM_MANAGE));
                string token = credential.JwToken;
                AccountManage profile = GetApiAccountManage.GetAccountManages(credential.JwToken).SingleOrDefault(p => p.Email == credential.Email);
                profile.Password = Encryptor.MD5Hash(pass1);
                using (HttpClient client = HelperClient.GetClient(token))
                {
                    client.BaseAddress = new Uri(Constants.BASE_URI);

                    var putTask = client.PutAsJsonAsync<AccountManage>(Constants.ACCOUNT_MANAGE + "/" + profile.Email, profile);
                    putTask.Wait();
                }
                return RedirectToAction("Index", "Home");
            }
            return View("ChangePassword");
        }
    }
}