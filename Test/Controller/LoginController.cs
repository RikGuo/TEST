using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Test.Content;
using Test.Model;
using Test.Model.Interface;

namespace Test.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserService UserIdentity;
        private readonly JwtHelpers jwt;
        public LoginController(IUserService _UserIdentity, JwtHelpers _jwt)
        {
            this.UserIdentity = _UserIdentity;
            this.jwt = _jwt;
        }
        
        [HttpPost]
        public async Task<EF_Login> Login(EF_User user)
        {

            var userIdentity = await this.UserIdentity.UserIdentityVerification(user.UserId, user.Password);

            EF_Login result = new EF_Login()
            {
                UserName = user.UserName
            };

            if (userIdentity.Role == "客戶")
            {
                result.Role = userIdentity.Role;
                result.UserId = userIdentity.UserId;
                result.UserName = userIdentity.UserName;
                result.Password = userIdentity.Password;
                var TokenStr = this.jwt.JwtGenerateToken(user);
                result.TokenData = TokenStr;
                result.Message = "登入成功";
            }
            else if(userIdentity.Role == "員工")
            {
                result.Role = userIdentity.Role;
                result.UserId = userIdentity.UserId;
                result.UserName = userIdentity.UserName;
                result.Password = userIdentity.Password;
                var TokenStr = this.jwt.JwtGenerateToken(user);
                result.TokenData = TokenStr;
                result.Message = "登入成功";
            }
            else if(userIdentity.Role == "管理者")
            {
                result.Role = userIdentity.Role;
                result.UserId = userIdentity.UserId;
                result.UserName = userIdentity.UserName;
                result.Password = userIdentity.Password;
                var TokenStr = this.jwt.JwtGenerateToken(user);
                result.TokenData = TokenStr;
                result.Message = "登入成功";
            }
            else
            {
                result.Message = "帳號或密碼輸入錯誤";
            }
            return result;
        }
    }
}