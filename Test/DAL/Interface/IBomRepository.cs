using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.EFORM;
using Test.Model;

namespace Test.DAO.Interface
{
    public interface IBomRepository
    {
        Task<Bom> GetProductid(int id);
        Task<bool> CheckBOM(int productId);
        IQueryable<EF_BOM> GetBOMInfo(string search);
        Task<RS_ModifyResult> CreateBOM(EF_BOM DataEntry);
        Task<RS_ModifyResult> UpdateBOM(EF_BOM DataEntry);
        Task<RS_ModifyResult> DeleteBOM(EF_BOM Id);
    }
}
