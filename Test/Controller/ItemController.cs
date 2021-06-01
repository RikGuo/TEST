using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Test.Logic.Interface;
using Test.Model;

namespace Test.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService item;
        public ItemController(IItemService _Item)
        {
            this.item = _Item;
        }
        [Authorize(Roles = "員工")]
        [HttpGet]
        public async Task<IActionResult> GetItem(int? page, string ItemName)
        {
            var r = await this.item.GetItem(page,ItemName);
            return new JsonResult(r);
        }
        [Authorize(Roles = "員工")]
        [HttpPost]
        public async Task<IActionResult> CreateItem(EF_Item DataEntry)
        {
            var r = await this.item.CreateItem(DataEntry);
            return new JsonResult(r);

        }
        [Authorize(Roles = "員工")]
        [HttpPut]
        public async Task<IActionResult> UpdateItem(EF_Item DataEntry)
        {
            var r = await this.item.UpdateItem(DataEntry);
            return new JsonResult(r);
        }
        [Authorize(Roles = "員工")]
        [HttpDelete]
        public async Task<IActionResult> DeleteItem(EF_Item id)
        {
            var r = await this.item.DeleteItem(id);
            return new JsonResult(r);
        }
    }
}