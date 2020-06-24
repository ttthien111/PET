using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using PETSHOP.Areas.Admin.Models;
using PETSHOP.Common;
using PETSHOP.Models.ModelView;

namespace PETSHOP.Areas.Admin.Controllers
{
    public class CheckAuthenticateAdminController : CheckAuthenticateManageController
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            CredentialManage credential = HttpContext.Session.GetObject<CredentialManage>(Constants.VM_MANAGE);
            if (credential.AccountRoleName != Constants.ADMIN)
            {
                filterContext.Result = new RedirectToRouteResult(new
                RouteValueDictionary(new { controller = "NotFound", action = "Index" }));
            }

            base.OnActionExecuting(filterContext);
        }
    }
}