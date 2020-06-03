using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace PETSHOP.Areas.Admin.Controllers
{
    public class CheckAuthenticationController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (HttpContext.Session.GetString("_") == null)
            {
                filterContext.Result = new RedirectToRouteResult(new
                  RouteValueDictionary(new { controller = "Admin", action = "Login" }));
            }
            base.OnActionExecuting(filterContext);
        }
    }
}