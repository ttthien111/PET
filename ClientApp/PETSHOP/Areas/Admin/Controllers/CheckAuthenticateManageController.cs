using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using PETSHOP.Common;
using PETSHOP.Models.ModelView;

namespace PETSHOP.Areas.Admin.Controllers
{
    public class CheckAuthenticateManageController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (HttpContext.Session.GetObject<CredentialModel>(Constants.VM_MANAGE) == null)
            {
                    filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new { controller = "Auth", action = "Index"}));
            }
            
            base.OnActionExecuting(filterContext);
        }
    }
}