using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using PETSHOP.Common;
using PETSHOP.Models.ModelView;

namespace PETSHOP.Controllers
{
    public class CheckAuthenForCheckOutController : CheckAuthenController
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (HttpContext.Session.GetObject<List<CartItem>>("cart") == null)
            {
                filterContext.Result = new RedirectToRouteResult(new
                  RouteValueDictionary(new { controller = "NotFound", action = "Index" }));
            }
            base.OnActionExecuting(filterContext);
        }
    }
}