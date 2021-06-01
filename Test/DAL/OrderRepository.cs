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
    public class OrderRepository:IOrderRepository
    {
        private readonly TestContext _dbcontext;

        public OrderRepository(TestContext dbcontext)
        {
            this._dbcontext = dbcontext;

        }
        protected Order Transfor(EF_Order DataEntry)
        {
            return new Order()
            {
                Id=DataEntry.Id,
                OrderSubject=DataEntry.OrderSubject,
                OrderApplicant=DataEntry.OrderApplicant,
                Status=DataEntry.Status,
                CreateDate = DataEntry.CreateDate,
                ModifyDate = DataEntry.ModifyDate
            };
        }
        public Task<EF_Order> GetStatus(string OrderSubject)
        {
            return this.GetOrders().Where(b => b.OrderSubject == OrderSubject).FirstOrDefaultAsync();
        }
        public IQueryable<EF_Order> GetOrders()
        {
            var linq_ef = this._dbcontext.Order.Select(c => new EF_Order
            {
                Id = c.Id,
                OrderSubject = c.OrderSubject,
                OrderApplicant = c.OrderApplicant,
                Status = c.Status,
                CreateDate = c.CreateDate,
                ModifyDate = c.ModifyDate
            });

            return linq_ef;
        }        
        
        public async Task<bool> CheckItem(int currentitem)
        {
            return await this._dbcontext.Item.Where(m => m.CurrentItem==currentitem).AnyAsync();
        }
        public IQueryable<EF_Item> GetItemquality(int currentitem)
        {
            var ef_linq = this._dbcontext.Item.Where(b => b.CurrentItem == currentitem).Select(b => new EF_Item
            {
                Id = b.Id,
                ItemName = b.ItemName,
                CurrentItem = b.CurrentItem,
                CreateDate = b.CreateDate,
                ModifyDate = b.ModifyDate
            });
            return ef_linq;
        }
        public IQueryable<EF_BOM> GetBOMquality(int id)
        {
            var ef_linq = this._dbcontext.Bom.Where(b => b.Autoid == id).Select(b => new EF_BOM
            {
                Autoid =b.Autoid,
                IdProduct = b.IdProduct,
                IdItem = b.IdItem,
                ItemNumber = b.ItemNumber,
                CreateDate = b.CreateDate,
                ModifyDate = b.ModifyDate
            });
            return ef_linq;
        }
        public async Task<RS_ModifyResult> CreateOrder(EF_Order DataEntry)
        {
            RS_ModifyResult result = new RS_ModifyResult("Add");
            try
            {
                await this._dbcontext.Order.AddAsync(this.Transfor(DataEntry));
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
        public async Task<RS_ModifyResult> UpdateOrder(EF_Order DataEntry)
        {
            RS_ModifyResult result = new RS_ModifyResult("Update");
            try
            {
                var clone = this.Transfor(DataEntry);
                var ef_find = await this._dbcontext.Order.FirstAsync(b => b.Id == DataEntry.Id);
                this._dbcontext.Entry(ef_find).CurrentValues.SetValues(clone);
                this._dbcontext.Order.Update(ef_find);                
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
        public async Task<RS_ModifyResult> DeleteOrder(int Id)
        {
            RS_ModifyResult result = new RS_ModifyResult("Delete");
            try
            {
                var ef_find = await this._dbcontext.Order.SingleAsync(b => b.Id == Id);
                this._dbcontext.Order.Remove(ef_find);
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
