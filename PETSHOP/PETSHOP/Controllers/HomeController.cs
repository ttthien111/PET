using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PETSHOP.Models;
using PETSHOP.Utils;

namespace PETSHOP.Controllers
{
    public class HomeController : Controller
    {
        public readonly PETSHOPContext _ctx;
        public HomeController(PETSHOPContext ctx)
        {
            _ctx = ctx;
        }
        public IActionResult Index()
        {
            ViewBag.Category = GetApiCategories.GetCategories().ToList(); // api
            int accountRoleIdCust = _ctx.AccountRole.SingleOrDefault(p => p.AccountRoleName == "Customer").AccountRoleId;
            ViewBag.InfoServices = GetApiProducts.GetProducts().ToList().Count() + "-" + _ctx.Account.Where(p => p.AccountRoleId == accountRoleIdCust).ToList().Count() + "-" + _ctx.Bill.ToList().Count();
            return View();
        }
    }
}
