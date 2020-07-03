using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ASPCore_Final.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PETSHOP.Areas.Admin.Models;
using PETSHOP.Common;
using PETSHOP.Models;
using PETSHOP.Models.ModelView;
using PETSHOP.Utils;

namespace PETSHOP.Areas.Admin.Controllers
{
    [Area(Constants.ADMIN)]
    public class AccountManagesController : CheckAuthenticateAdminController
    {
        public IActionResult Index()
        {
            CredentialManage credential = JsonConvert.DeserializeObject<CredentialManage>(HttpContext.Session.GetString(Constants.VM_MANAGE));
            List<AccountManageDTO> accounts = GetApiAccountManage.GetAccountManages(credential.JwToken)
                                                .Select(p => new AccountManageDTO()
                                                {
                                                    Email = p.Email,
                                                    AccountRoleName = GetApiAccountRoles.GetAccountRoles().SingleOrDefault(k => k.AccountRoleId == p.AccountRoleId).AccountRoleName,
                                                    Password = p.Password,
                                                    FullName = p.FullName,
                                                    IsActivated = p.IsActivated,
                                                    Avatar = p.Avatar,
                                                    Address = p.Address

                                                }).ToList();

            return View(accounts);
        }


        public IActionResult Create()
        {
            ViewBag.AccountRoleName = GetApiAccountRoles.GetAccountRoles().ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(AccountManageDTO dto, IFormFile Avatar)
        {
            var obj = dto;
            if (dto.Password == null) { return NoContent(); }

            AccountManage accountManage = new AccountManage()
            {
                FullName = dto.FullName,
                Address = dto.Address,
                Email = dto.Email,
                IsActivated = dto.IsActivated,
                Password = Encryptor.MD5Hash(dto.Password),
                AccountRoleId = GetApiAccountRoles.GetAccountRoles().SingleOrDefault(q => q.AccountRoleName == dto.AccountRoleName).AccountRoleId
            };

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
                    accountManage.Avatar = accountImg + extension;
                }
                else
                {
                    ModelState.AddModelError("", Constants.EXTENSION_IMG_NOT_SUPPORT);
                    return Content(Constants.EXTENSION_IMG_NOT_SUPPORT);
                }
            }
            else
            {
                accountManage.Avatar = "denyPaw.png";
            }
            

            //account avatar 
            CredentialManage credential = JsonConvert.DeserializeObject<CredentialManage>(HttpContext.Session.GetString(Constants.VM_MANAGE) != null ? HttpContext.Session.GetString(Constants.VM_MANAGE) : "");
            string token = credential.JwToken;

            using (HttpClient client = HelperClient.GetClient(token))
            {
                client.BaseAddress = new Uri(Common.Constants.BASE_URI);

                var postTask = client.PostAsJsonAsync<AccountManage>(Constants.ACCOUNT_MANAGE, accountManage);
                postTask.Wait();

                var result = postTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Product>();
                    readTask.Wait();

                   
                }
                return RedirectToAction(nameof(Index));
            }
            
        }
        public IActionResult ActivateAccount(string accountEmail)
        {
            CredentialManage credential = JsonConvert.DeserializeObject<CredentialManage>(HttpContext.Session.GetString(Constants.VM_MANAGE) != null ? HttpContext.Session.GetString(Constants.VM_MANAGE) : "");
            string token = credential.JwToken;

            AccountManage acc = GetApiAccountManage.GetAccountManages(token).SingleOrDefault(p => p.Email == accountEmail);
                
            // update status
            acc.IsActivated = !acc.IsActivated;

            using (HttpClient client = HelperClient.GetClient(token))
            {
                client.BaseAddress = new Uri(Constants.BASE_URI);

                var putTask = client.PutAsJsonAsync<AccountManage>(Constants.ACCOUNT_MANAGE + "/" + acc.Email, acc);
                putTask.Wait();

                var result = putTask.Result;
            }

            return RedirectToAction("Index");
        }
 
        public IActionResult ResetPassword(string accountEmail)
        {
            CredentialManage credential = JsonConvert.DeserializeObject<CredentialManage>(HttpContext.Session.GetString(Constants.VM_MANAGE) != null ? HttpContext.Session.GetString(Constants.VM_MANAGE) : "");
            string token = credential.JwToken;
             
            AccountManage acc = GetApiAccountManage.GetAccountManages(token).SingleOrDefault(p => p.Email == accountEmail);


            string newPassword = Encryptor.RandomString(6);
            acc.Password = Encryptor.MD5Hash(newPassword);

            using (HttpClient client = HelperClient.GetClient(token))
            {
                client.BaseAddress = new Uri(Constants.BASE_URI);
                var putTask = client.PutAsJsonAsync<AccountManage>(Constants.ACCOUNT_MANAGE + "/" + acc.Email, acc);
                putTask.Wait();
                var result = putTask.Result;
            }
            //send Email
            SenderEmail.SendMail(accountEmail, "PETSHOP - Reset Your Password", String.Format("Your new password is here {0} please check it",newPassword));

            return NoContent();
        }
    }
}