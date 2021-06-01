using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Test.Model;
using Test.Model.Interface;

namespace Test.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService user;
        public UserController(IUserService _User)
        {
            this.user = _User;
        }
        [Authorize(Roles = "管理者")]
        [HttpPost]
        public async Task<IActionResult> CreateUser(EF_User userIdentity)
        {
            var r = await this.user.CreateUser(userIdentity);
            return new JsonResult(r);
        }
        [Authorize(Roles = "管理者")]
        [HttpPut]
        public async Task<RS_Object> UpdateUser(EF_User userIdentity)
        {
            return await this.user.UpdateUser(userIdentity);
        }
    }
}