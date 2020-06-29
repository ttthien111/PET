
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
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using PETSHOP.Models.DataTransferObject;

namespace PETSHOP.Services
{
    public interface ILoginService
    {
        Account Authenticate(string username, string password);
        IEnumerable<Account> GetAll();
        Account AuthenticateExternal(AuthenticateExternal external);
        string GetRoleName(int accountIdRole);
        AccountManageDTO AuthenticateManage(string email, string password);
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
            var user = _context.Account.SingleOrDefault(x => x.AccountUserName == username && x.AccountPassword == Helper.Encryptor.MD5Hash(password) && x.IsActive == true);
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.AccountId.ToString()),
                    new Claim(ClaimTypes.Role, GetRoleName(user.AccountRoleId))
                }),
                Expires = DateTime.UtcNow.AddHours(6),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Jwtoken = tokenHandler.WriteToken(token);

            UserProfile profile = _context.UserProfile.SingleOrDefault(x => x.AccountId == user.AccountId);
            user.UserProfile.Add(profile);

            return user;
            throw new NotImplementedException();
        }

        public AccountManageDTO AuthenticateManage(string email, string password)
        {
            var user = _context.AccountManage.Where(x => x.Email == email && x.Password == Helper.Encryptor.MD5Hash(password) && x.IsActivated == true)
                .Select(p=> new AccountManageDTO()
            {
                Email = p.Email,
                Password = Encryptor.MD5Hash(p.Password),
                AccountRoleId = p.AccountRoleId,
                Address = p.Address,
                Avatar = p.Avatar,
                FullName = p.FullName,
                IsActivated = p.IsActivated
            }).ToList();

            if (user.Count() > 0)
            {
                if (user[0] == null)
                {
                    return null;
                }
                else
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                    new Claim(ClaimTypes.Name, user[0].Email.ToString()),
                    new Claim(ClaimTypes.Role, GetRoleName(user[0].AccountRoleId))
                        }),
                        Expires = DateTime.UtcNow.AddHours(6),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    user[0].Jwtoken = tokenHandler.WriteToken(token);

                    return user[0];
                    throw new NotImplementedException();
                }
            }
            else
            {
                return null;
            }
            // authentication successful so generate jwt token
           
        }

        public Account AuthenticateExternal(AuthenticateExternal external)
        {
            var user = _context.Account.SingleOrDefault(x => x.AccountUserName == external.Email && x.IsActive == true);

            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.AccountId.ToString()),
                    new Claim(ClaimTypes.Role, GetRoleName(user.AccountRoleId))
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

        public string GetRoleName(int accountRoleId)
        {
            string role = _context.AccountRole.SingleOrDefault(p=> p.AccountRoleId == accountRoleId).AccountRoleName;
            return role;
        }

        public IEnumerable<Account> GetAll()
        {       
            throw new NotImplementedException();
        }
    }
}
