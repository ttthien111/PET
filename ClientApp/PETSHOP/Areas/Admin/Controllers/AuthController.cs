using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using PETSHOP.Models;
using Microsoft.AspNetCore.Mvc;
using PETSHOP.Areas.Admin.Models;
using PETSHOP.Common;
using Microsoft.AspNetCore.Http;

namespace PETSHOP.Areas.Admin.Controllers
{
    [Area("admin")]
    public class AuthController : Controller
    {
        public readonly PETSHOPContext _ctx;
        public AuthController(PETSHOPContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet("/Admin/Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("/Admin/Login")]
        public IActionResult Login(LoginModel login)
        {
            if (login != null)
            {
                Account userLogin = _ctx.Account.SingleOrDefault(p => p.AccountUserName == login.Username || p.AccountPassword == Encryptor.SHA256Hash(login.Password));
                if(userLogin != null)
                {
                    InfoUserLoginModel infoUserLogin = new InfoUserLoginModel()
                    {
                        Email = userLogin.AccountUserName,
                        Role = _ctx.AccountRole.Find(userLogin.AccountRoleId).AccountRoleName,
                        LoginAt = DateTime.Now
                    };

                    HttpContext.Session.SetString("_", infoUserLogin.Email + "," + infoUserLogin.Role + "," + infoUserLogin.LoginAt);
                    return LocalRedirect("/admin");
                } else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không chính xác !");
                    return View("Index");
                }
            }
            else
            {
                ModelState.AddModelError("", "Vui lòng nhập email và mật khẩu");
                return View("Index");
            }
        }

        [HttpGet("/Admin/Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("_");
            return LocalRedirect("/admin");
        }
    }
}