using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Content;
using Test.DAO.Interface;
using Test.Logic.Interface;
using Test.Model;
using X.PagedList;

namespace Test.Logic
{
    public class ItemService:IItemService
    {
        private readonly IItemRepository DaoItem;
        private const int pageSize = 10;
        public ItemService(IItemRepository daoItem)
        {
            this.DaoItem = daoItem;
        }
        private async Task<bool> CheckCurrentItem(EF_Item DataEntry)
        {
            var temp = await this.DaoItem.GetItem(DataEntry.Id);
            //判斷是否物料表曾被使用過或有庫存貨是有關連到產品
            return temp.CurrentItem == DataEntry.CurrentItem? false : true;
        }
        public async Task<RS_Item> GetItem(int? page, string ItemName)
        {
            RS_Item result = new RS_Item();
            try
            {
                var pageIndex = page ?? 1;
                var products = this.DaoItem.GetItems();

                if (!string.IsNullOrEmpty(ItemName))
                    products = products.Where(m => m.ItemName.Contains(ItemName));
                result.Count = products.Count();
                result.PageSize = pageSize;
                result.ItemPagedlsit = await products.ToPagedListAsync(pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                Nlogger.WriteLog(Nlogger.NType.Error, $"{ex.Message}{ex.InnerException}", ex);
            }
            return result;
        }
        public async Task<RS_Object> CreateItem(EF_Item DataEntry)
        {
            RS_Object result = new RS_Object();
            try
            {
                var Rs_Modify = await this.DaoItem.CreateItem(DataEntry);
                Rs_Modify.Message = Rs_Modify.Success ? $"成功新增物料資料{Rs_Modify.Count}筆" : Rs_Modify.Message;               
                result = Rs_Modify.Transfor("物料");
            }
            catch (Exception ex)
            {
                Nlogger.WriteLog(Nlogger.NType.Error, $"{ex.Message}{ex.InnerException}", ex);
            }
            return result;
        }

        public async Task<RS_Object> UpdateItem(EF_Item DataEntry)
        {
            RS_Object result = new RS_Object();
            try
            {
                var Rs_Modify = await this.DaoItem.UpdateItem(DataEntry);
                Rs_Modify.Message = Rs_Modify.Success ? $"成功更新物料資料{Rs_Modify.Count}筆" : Rs_Modify.Message;
                result = Rs_Modify.Transfor("物料");
                return result;
            }
            catch (Exception ex)
            {
                Nlogger.WriteLog(Nlogger.NType.Error, $"{ex.Message}{ex.InnerException}", ex);
            }
            return result;
        }

        public async Task<RS_Object> DeleteItem(EF_Item currentitem)
        {
            RS_Object result = new RS_Object();
            try
            {
                RS_ModifyResult FinData = new RS_ModifyResult("Check")
                {
                    Success = true
                };
                if (await this.CheckCurrentItem(currentitem))
                    FinData = await this.CheckItem(currentitem.CurrentItem);
                if (FinData.Success)
                {
                    FinData = await this.DaoItem.DeleteItem(currentitem);
                    FinData.Message = $"刪除成功";
                }
                result = FinData.Transfor("Item");
                Nlogger.WriteLog(Nlogger.NType.Info, FinData.Message);
            }
            catch (Exception ex)
            {
                Nlogger.WriteLog(Nlogger.NType.Error, $"{ex.Message}{ex.InnerException}", ex);
            }
            return result;
        }

        private async Task<RS_ModifyResult> CheckItem(int Product)
        {
            bool checkProductId = await this.DaoItem.CheckItem(Product);
            if (checkProductId)
                return new RS_ModifyResult("Check")
                {
                    Count = 0,
                    Message = "物料曾被使用過或物料留有庫存",
                    Success = false
                };
            else
                return new RS_ModifyResult("Check")
                {
                    Success = true
                };
        }
    }
}
