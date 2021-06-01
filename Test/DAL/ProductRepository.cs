using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Content;
using Test.DAO.Interface;
using Test.EFORM;
using Test.Model;

namespace Test.DAO
{
    public class ProductRepository : IProductRepository
    {
        private readonly TestContext dbcontext;

        public ProductRepository(TestContext _dbcontext)
        {
            this.dbcontext = _dbcontext;
        }
        protected Product Transfor(EF_Product DataEntry)
        {

            return new Product()
            {
                Id = DataEntry.Id,
                ProductName=DataEntry.ProductName,
                ProductPrice=DataEntry.ProductPrice,
                CreateDate = DataEntry.CreateDate,
                ModifyDate = DataEntry.ModifyDate
            };
        }               
        public string GetProductCount(int id)
        {
            return this.dbcontext.Product.OrderByDescending(b => b.Id == id).Select(b => b.ProductName).First();
        }
        public async Task<bool> CheckOrder(int id)
        {
            return await this.dbcontext.Order.Where(m => m.Id == id).AnyAsync();
        }
        public async Task<Order>GetOrder(int id)
        {
            return await this.dbcontext.Order.Where(m => m.Id == id).FirstOrDefaultAsync();
        }
        public async Task<bool> CheckProduct(string productname)
        {
            return await this.dbcontext.Product.Where(m => m.ProductName == productname).AnyAsync();
        }

        public IQueryable<EF_Product> GetProducts()
        {
            var linq_ef = this.dbcontext.Product.Select(c => new EF_Product
            {
                Id = c.Id,
                ProductName=c.ProductName,
                ProductPrice=c.ProductPrice,
                CreateDate = c.CreateDate,
                ModifyDate = c.ModifyDate
            });
            return linq_ef;
        }

        public async Task<RS_ModifyResult> CreateProduct(EF_Product DataEntry)
        {
            RS_ModifyResult result = new RS_ModifyResult("Add");
            try
            {
                this.dbcontext.Product.Add(this.Transfor(DataEntry));
                var SaveResult = await this.dbcontext.SaveChangesAsync();
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
        public async Task<RS_ModifyResult> UpdateProduct(EF_Product DataEntry)
        {
            RS_ModifyResult result = new RS_ModifyResult("Update");
            try
            {
                var clone = this.Transfor(DataEntry);
                var ef_find = await this.dbcontext.Product.FirstAsync(b => b.Id == DataEntry.Id);
                this.dbcontext.Entry(ef_find).CurrentValues.SetValues(clone);
                this.dbcontext.Product.Update(ef_find);
                var SaveResult = await this.dbcontext.SaveChangesAsync();
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
        public async Task<RS_ModifyResult> DeleteProduct(EF_Product Id)
        {
            RS_ModifyResult result = new RS_ModifyResult("Delete");
            try
            {
                var ef_find = await this.dbcontext.Product.SingleAsync(b => b.Id == Id.Id);
                this.dbcontext.Product.Remove(ef_find);
                var SaveResult = await this.dbcontext.SaveChangesAsync();
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
