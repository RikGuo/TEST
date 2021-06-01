using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Test.Logic.Interface;
using Test.Model;

namespace Test.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BomController : ControllerBase
    {
        private readonly IBOMService bom;
        public BomController(IBOMService _BOM)
        {
            this.bom = _BOM;
        }
        [HttpGet]
        public async Task<IActionResult> GetBOMInfo(int? page, string search)
        {
            var r = await this.bom.GetBOMInfo(page, search);
            return new JsonResult(r);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBOM(EF_BOM DataEntry)
        {
            var r = await this.bom.CreateBOM(DataEntry);
            return new JsonResult(r);

        }
        [HttpPut]
        public async Task<IActionResult> UpdateBOM(EF_BOM DataEntry)
        {
            var r = await this.bom.UpdateBOM(DataEntry);
            return new JsonResult(r);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteBOM(EF_BOM productid)
        {
            var r = await this.bom.DeleteBOM(productid);
            return new JsonResult(r);
        }
    }
}