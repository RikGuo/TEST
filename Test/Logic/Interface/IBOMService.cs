using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Model;

namespace Test.Logic.Interface
{
    public interface IBOMService
    {
        
        Task<RS_BOM> GetBOMInfo(int? page, string search);
        Task<RS_Object> CreateBOM(EF_BOM DataEntry);
        Task<RS_Object> UpdateBOM(EF_BOM DataEntry);
        Task<RS_Object> DeleteBOM(EF_BOM productid);
    }
}
