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
    public class ProductController : ControllerBase
    {
        private readonly IProductService product;
        public ProductController(IProductService _Product)
        {
            this.product = _Product;
        }
        [Authorize(Roles = "員工")]
        [HttpGet]
        public async Task<IActionResult> GetProduct(int? page, string ProductName)
        {
            var r = await this.product.GetProduct(page, ProductName);
            return new JsonResult(r);
        }
        [Authorize(Roles = "員工")]
        [HttpPost]
        public async Task<IActionResult> CreateProduct(EF_Product DataEntry)
        {
            var r = await this.product.CreateProduct(DataEntry);
            return new JsonResult(r);

        }
        [Authorize(Roles = "員工")]
        [HttpPut]
        public async Task<IActionResult> UpdateProduct(EF_Product DataEntry)
        {
            var r = await this.product.UpdateProduct(DataEntry);
            return new JsonResult(r);
        }
        [Authorize(Roles = "員工")]
        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(EF_Product product, EF_Order order)
        {
            var r = await this.product.DeleteProduct(product,order);
            return new JsonResult(r);
        }
    }
}