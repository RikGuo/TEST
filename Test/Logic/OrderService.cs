using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Content;
using Test.DAO.Interface;
using Test.EFORM;
using Test.Model;
using X.PagedList;

namespace Test.Logic.Interface
{
    public class OrderService :IOrderService
    {
        private readonly IOrderRepository DaoOrder;
        private readonly IMailService Mailservice;
        private const int pageSize = 10;
        public OrderService(IOrderRepository daoOrder,IMailService mailService)
        {
            this.DaoOrder = daoOrder;
            this.Mailservice = mailService;
        }
        public async Task<RS_Order> GetOrderInfo(int? page)
        {
            RS_Order result = new RS_Order();
            try
            {
                var pageIndex = page ?? 1;                
                var order = this.DaoOrder.GetOrders();
                result.Count = order.Count();
                result.PageSize = pageSize;
                result.OrderPagedlsit = await order.OrderByDescending(b => b.Id).ToPagedListAsync(pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                Nlogger.WriteLog(Nlogger.NType.Error, $"{ex.Message}{ex.InnerException}", ex);
            }
            return result;
        }

        public async Task<RS_Object> CreateOrder(EF_Order DataEntry,bool sendemail)
        {
            RS_Object result = new RS_Object();
            try
            { 
                var Rs_Modify = await this.DaoOrder.CreateOrder(DataEntry);
                DataEntry.Status = "成立";
                Rs_Modify = this.Mailservice.SendEmailAsync(sendemail);                
                Rs_Modify.Message = Rs_Modify.Success ? $"成功新增訂單資料{Rs_Modify.Count}筆" : Rs_Modify.Message;
                result = Rs_Modify.Transfor("訂單");

            }
            catch (Exception ex)
            {
                Nlogger.WriteLog(Nlogger.NType.Error, $"{ex.Message}{ex.InnerException}", ex);
            }
            return result;
        }

        public async Task<RS_Object> UpdateOrder(EF_Order DataEntry,EF_Item Item,EF_BOM BOM)
        {
            RS_Object result = new RS_Object();
            try
            {
                var Rs_Modify = await this.CheckItem(DataEntry.Id);
                var Itemnumber = this.DaoOrder.GetItemquality(Item.CurrentItem);
                var BOMnumber = this.DaoOrder.GetBOMquality(BOM.Autoid);
                if (Rs_Modify.Success&&Itemnumber.Count()>0)
                {
                    Rs_Modify = await this.DaoOrder.UpdateOrder(DataEntry);
                    DataEntry.Status = "生產中";                    
                    Rs_Modify.Message = Rs_Modify.Success ? $"成功更新訂單資料{Rs_Modify.Count}筆" : Rs_Modify.Message;
                    result.Count = Itemnumber.Count() -BOMnumber.Count();
                    result = Rs_Modify.Transfor("訂單");
                }
                else
                    result.Message = "訂單不足以生產";
            }
            catch (Exception ex)
            {
                Nlogger.WriteLog(Nlogger.NType.Error, $"{ex.Message}{ex.InnerException}", ex);
            }
            return result;
        }

        public async Task<RS_Object> DeleteOrder(int productid)
        {
            RS_Object result = new RS_Object();
            try
            {
                var Rs_Modify = await this.DaoOrder.DeleteOrder(productid);
                Rs_Modify.Message = Rs_Modify.Success ? $"成功刪除訂單資料{Rs_Modify.Count}筆" : Rs_Modify.Message;
                result = Rs_Modify.Transfor("訂單");
            }
            catch (Exception ex)
            {
                Nlogger.WriteLog(Nlogger.NType.Error, $"{ex.Message}{ex.InnerException}", ex);
            }
            return result;
        }
        private List<int> GetOrderNumber(EF_Order number)
        {
            List<int> result = new List<int>();
            string str = "O"+DateTime.Now.ToString("yyyyMMDD"); 
            int z = int.Parse(number.Id.ToString().Replace(str, "")) + 1;
            str += z.ToString().PadLeft(4, '0');
            for (int i = 0; i < str.Length; i++)
                result.Add(int.Parse(str));
            return result;
        }
        private async Task<RS_ModifyResult> CheckItem(int Product)
        {
            bool checkBOMid = await this.DaoOrder.CheckItem(Product);
            if (checkBOMid)
                return new RS_ModifyResult("Check")
                {
                    Success = true
                };
            else
                return new RS_ModifyResult("Check")
                {
                    Count = 0,
                    Success = false
                };
        }
    }
}
