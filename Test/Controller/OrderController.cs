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
    public class OrderController : ControllerBase
    {
        private readonly IOrderService order;
        public OrderController(IOrderService _Order)
        {
            this.order = _Order;
        }
        [HttpGet]
        public async Task<IActionResult> GetOrderInfo(int? page)
        {
            var r = await this.order.GetOrderInfo(page);
            return new JsonResult(r);
        }
        [Authorize(Roles = "客戶")]
        [HttpPost]
        public async Task<IActionResult> CreateOrder(EF_Order DataEntry,bool sendemail)
        {
            var r = await this.order.CreateOrder(DataEntry,sendemail);
            return new JsonResult(r);
        }
        [HttpPut]
        public async Task<RS_Object> UpdateOrder(EF_Order DataEntry,EF_Item Item)
        {
            var r = await this.order.UpdateOrder(DataEntry,Item);
            return new JsonResult(r);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteOrder(int productid)
        {
            var r = await this.order.DeleteOrder(productid);
            return new JsonResult(r);
        }
    }
}