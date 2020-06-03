﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PETSHOP.Areas.Admin.Controllers
{
    [Area("admin")]
    public class HomeController : CheckAuthenticationController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}