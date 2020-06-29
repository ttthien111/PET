using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
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

    }
}