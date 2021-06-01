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
    public class ItemRepository:IItemRepository
    {
        private readonly TestContext dbcontext;

        public ItemRepository(TestContext _dbcontext)
        {
            this.dbcontext = _dbcontext;
        }
        protected Item Transfor(EF_Item DataEntry)
        {
            return new Item()
            {
                Id=DataEntry.Id,
                ItemName=DataEntry.ItemName,
                CurrentItem=DataEntry.CurrentItem,
                CreateDate = DataEntry.CreateDate,
                ModifyDate = DataEntry.ModifyDate
            };
        }               
        public string GetItemCount(int id)
        {
            return this.dbcontext.Item.OrderByDescending(b => b.Id == id).Select(b => b.ItemName).First();
        }
        public async Task<bool> CheckItem(int ItemId)
        {
            return await this.dbcontext.Item.Where(m => m.Id == ItemId).AnyAsync();
        }
        public async Task<Item> GetItem(int id)
        {
            return await this.dbcontext.Item.Where(m => m.Id == id).FirstOrDefaultAsync();
        }
        public IQueryable<EF_Item> GetItems()
        {
            var linq_ef = this.dbcontext.Item.Select(c => new EF_Item
            {
                Id = c.Id,
                ItemName=c.ItemName,
                CurrentItem=c.CurrentItem,
                CreateDate = c.CreateDate,
                ModifyDate = c.ModifyDate
            });

            return linq_ef;
        }
        public async Task<RS_ModifyResult> CreateItem(EF_Item DataEntry)
        {
            RS_ModifyResult result = new RS_ModifyResult("Add");
            try
            {
                await this.dbcontext.Item.AddAsync(this.Transfor(DataEntry));
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
        public async Task<RS_ModifyResult> UpdateItem(EF_Item DataEntry)
        {
            RS_ModifyResult result = new RS_ModifyResult("Update");
            try
            {
                var clone = this.Transfor(DataEntry);
                var ef_find = await this.dbcontext.Item.FirstAsync(b => b.Id == DataEntry.Id);
                this.dbcontext.Entry(ef_find).CurrentValues.SetValues(clone);
                this.dbcontext.Item.Update(ef_find);
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
        public async Task<RS_ModifyResult> DeleteItem(EF_Item Id)
        {
            RS_ModifyResult result = new RS_ModifyResult("Delete");
            try
            {
                var ef_find = await this.dbcontext.Item.SingleAsync(b => b.Id == Id.Id);
                this.dbcontext.Item.Remove(ef_find);
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
