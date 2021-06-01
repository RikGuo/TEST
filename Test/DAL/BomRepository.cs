using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Test.Content;
using Test.DAO.Interface;
using Test.EFORM;
using Test.Model;

namespace Test.DAO
{
    public class BomRepository : IBomRepository
    {
        private readonly TestContext _dbcontext;

        public BomRepository(TestContext dbcontext)
        {
            this._dbcontext = dbcontext;

        }
        protected Bom Transfor(EF_BOM DataEntry)
        {
            return new Bom()
            {
                Autoid=DataEntry.Autoid,
                IdProduct=DataEntry.IdProduct,
                IdItem=DataEntry.IdItem,
                ItemNumber=DataEntry.ItemNumber,
                CreateDate = DataEntry.CreateDate,
                ModifyDate = DataEntry.ModifyDate
            };
        }
        public async Task<Bom> GetProductid(int id)
        {
            return await this._dbcontext.Bom.Where(m => m.IdProduct == id).FirstOrDefaultAsync();
        }
        public async Task<bool> CheckBOM(int productId)
        {
            return await this._dbcontext.Bom.Where(m =>m.IdProduct== productId).AnyAsync();
        }
        public IQueryable<EF_BOM> GetBOMInfo(string search)
        {
            var data = from a in _dbcontext.Bom
                       join b in _dbcontext.Product on a.IdProduct equals b.Id into ab
                       join c in _dbcontext.Item on a.IdItem equals c.Id into ac
                       from t in ab.DefaultIfEmpty()
                       where a.IdProduct.ToString().Contains(search) || string.IsNullOrEmpty(search)
                       from s in ac.DefaultIfEmpty()
                       where a.IdItem.ToString().Contains(search)||string.IsNullOrEmpty(search)
                       select new EF_BOM
                       {
                           Autoid = a.Autoid,
                           IdProduct=t.Id,
                           IdItem=s.Id,
                           ItemNumber=a.ItemNumber,
                           CreateDate = a.CreateDate,
                           ModifyDate = a.ModifyDate
                       };
            return data;
        }        
        public async Task<RS_ModifyResult> CreateBOM(EF_BOM DataEntry)
        {
            RS_ModifyResult result = new RS_ModifyResult("Add");
            try
            {
                await this._dbcontext.Bom.AddAsync(this.Transfor(DataEntry));
                var SaveResult = await this._dbcontext.SaveChangesAsync();
                result.Count = SaveResult;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                result.Success = false;
                Nlogger.WriteLog(Nlogger.NType.Error, ex.Message, ex);
            }
            return result;
        }
        public async Task<RS_ModifyResult> UpdateBOM(EF_BOM DataEntry)
        {
            RS_ModifyResult result = new RS_ModifyResult("Update");
            try
            {
                var clone = this.Transfor(DataEntry);
                var ef_find = await this._dbcontext.Bom.FirstAsync(b => b.Autoid == DataEntry.Autoid);
                this._dbcontext.Entry(ef_find).CurrentValues.SetValues(clone);
                this._dbcontext.Bom.Update(ef_find);
                var SaveResult = await this._dbcontext.SaveChangesAsync();
                result.Count = SaveResult;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                result.Success = false;
                Nlogger.WriteLog(Nlogger.NType.Error, ex.Message, ex);
            }
            return result;
        }
        public async Task<RS_ModifyResult> DeleteBOM(EF_BOM Id)
        {
            RS_ModifyResult result = new RS_ModifyResult("Delete");
            try
            {
                var ef_find = await this._dbcontext.Bom.SingleAsync(b => b.Autoid == Id.Autoid);
                this._dbcontext.Bom.Remove(ef_find);
                var SaveResult = await this._dbcontext.SaveChangesAsync();
                result.Count = SaveResult;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                result.Success = false;
                Nlogger.WriteLog(Nlogger.NType.Error, ex.Message, ex);
            }
            return result;
        }
    }
}
