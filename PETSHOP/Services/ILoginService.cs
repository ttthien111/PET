﻿
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PETSHOP.Helper;
using PETSHOP.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication;
using System.Security.Policy;
using PETSHOP.Models.LoginModel;

namespace PETSHOP.Services
{
    public interface ILoginService
    {
        Account Authenticate(string username, string password);
        IEnumerable<Account> GetAll();
        Account AuthenticateExternal(AuthenticateExternal external);
    }

    public class LoginService : ILoginService
    {
        private readonly PETSHOPContext _context;
        private readonly AppSetting _appSettings;

        public LoginService(IOptions<AppSetting> appSettings, PETSHOPContext context)
        {
            _appSettings = appSettings.Value;
            _context = context;
        }

        public Account Authenticate(string username, string password)
        {
            var user = _context.Account.SingleOrDefault(x => x.AccountUserName == username && x.AccountPassword == Helper.Encryptor.MD5Hash(password));
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Name, user.AccountId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Jwtoken = tokenHandler.WriteToken(token);

            UserProfile profile = _context.UserProfile.SingleOrDefault(x => x.AccountId == user.AccountId);
            user.UserProfile.Add(profile);

            return user;
            throw new NotImplementedException();
        }

        public Account AuthenticateExternal(AuthenticateExternal external)
        {
            var user = _context.Account.SingleOrDefault(x => x.AccountUserName == external.Email);

            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Name, user.AccountId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            user.Jwtoken = tokenHandler.WriteToken(token);
            UserProfile profile = _context.UserProfile.SingleOrDefault(x => x.AccountId == user.AccountId);
            user.UserProfile.Add(profile);

            return user;
           
        }

        public IEnumerable<Account> GetAll()
        {       
            throw new NotImplementedException();
        }
    }
}