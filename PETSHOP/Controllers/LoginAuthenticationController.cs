using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PETSHOP.Models;
using PETSHOP.Models.LoginModel;
using PETSHOP.Services;

namespace PETSHOP.Controllers
{
   
    [ApiController]
    [Route("api/[controller]")]
    public class LoginAuthenticationController : ControllerBase
    {
        private ILoginService _user;
        private readonly PETSHOPContext _context;

        public LoginAuthenticationController(ILoginService userService, PETSHOPContext context)
        {
            _user = userService;
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateModel model)
        {
            var user = _user.Authenticate(model.Username, model.Password);
            if (user == null)
                return BadRequest(new { message = "Wrong" });
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("AuthenticateManage")]
        public IActionResult AuthenticateManage([FromBody] AuthenticateModel model)
        {
            var user = _user.AuthenticateManage(model.Username, model.Password);
            if (user == null)
                return BadRequest(new { message = "Wrong" });
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("AuthenticateExternal")]
        public IActionResult AuthenticateExternal([FromBody] AuthenticateExternal model)
        {
            var user = _user.AuthenticateExternal(model);
            if (user == null)
                return BadRequest(new { message = "Wrong" });
            return Ok(user);
        }
    }
}