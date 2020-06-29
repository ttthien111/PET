using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PETSHOP.Common;

namespace PETSHOP.Areas.Admin.Controllers
{
    [Area(Constants.ADMIN)]
    public class StocksController : CheckAuthenticateManageController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}