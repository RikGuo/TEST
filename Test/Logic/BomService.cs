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
    public class BomService:IBOMService
    {
        private readonly IBomRepository Daobom;
        private const int pageSize = 10;
        public BomService(IBomRepository daobom)
        {
            this.Daobom = daobom;
        }
        private async Task<bool> CheckProduct(EF_BOM DataEntry)
        {
            var temp = await this.Daobom.GetProductid(DataEntry.IdProduct);
            //判斷曾被生產過
            return temp.IdProduct == DataEntry.IdProduct ? false : true;
        }
        public async Task<RS_BOM> GetBOMInfo(int?page,string search)
        {
            RS_BOM result = new RS_BOM();
            try
            {
                var pageIndex = page ?? 1;
                var ef_linq = this.Daobom.GetBOMInfo(search);
                result.Count = ef_linq.Count();
                result.PageSize = pageSize;
                result.BOMPagedlsit = await ef_linq.OrderByDescending(b => b.Autoid).ToPagedListAsync(pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                Nlogger.WriteLog(Nlogger.NType.Error, $"{ex.Message}{ex.InnerException}", ex);
            }
            return result;
        }
        
        public async Task<RS_Object> CreateBOM(EF_BOM DataEntry)
        {
            RS_Object result = new RS_Object();
            try
            {
                var Rs_Modify = await this.CheckBOM(DataEntry.Autoid);
                if (Rs_Modify.Success)
                {
                    Rs_Modify = await this.Daobom.CreateBOM(DataEntry);
                    Rs_Modify.Message = Rs_Modify.Success ? $"成功新增BOM資料{Rs_Modify.Count}筆" : Rs_Modify.Message;

                }
                result = Rs_Modify.Transfor("BOM");
            }
            catch (Exception ex)
            {
                Nlogger.WriteLog(Nlogger.NType.Error, $"{ex.Message}{ex.InnerException}", ex);
            }
            return result;
        }

        public async Task<RS_Object> UpdateBOM(EF_BOM DataEntry)
        {
            RS_Object result = new RS_Object();
            try
            {
                var Rs_Modify = await this.Daobom.UpdateBOM(DataEntry);
                Rs_Modify.Message = Rs_Modify.Success ? $"成功更新BOM資料{Rs_Modify.Count}筆" : Rs_Modify.Message;
                result = Rs_Modify.Transfor("BOM");
                return result;
            }
            catch (Exception ex)
            {
                Nlogger.WriteLog(Nlogger.NType.Error, $"{ex.Message}{ex.InnerException}", ex);
            }
            return result;
        }

        public async Task<RS_Object> DeleteBOM(EF_BOM productid)
        {
            RS_Object result = new RS_Object();
            try
            {
                RS_ModifyResult FinData = new RS_ModifyResult("Check")
                {
                    Success = true
                };
                if (await this.CheckProduct(productid))
                    FinData = await this.CheckBOM(productid.IdProduct);
                if (FinData.Success)
                {                   
                    FinData = await this.Daobom.DeleteBOM(productid);
                    FinData.Message = $"刪除成功";
                }
                result = FinData.Transfor("BOM");
                Nlogger.WriteLog(Nlogger.NType.Info, FinData.Message);
            }
            catch (Exception ex)
            {
                Nlogger.WriteLog(Nlogger.NType.Error, $"{ex.Message}{ex.InnerException}", ex);
            }
            return result;
        }


        private async Task<RS_ModifyResult> CheckBOM(int Product)
        {
            bool checkBOMid = await this.Daobom.CheckBOM(Product);
            if (checkBOMid)
                return new RS_ModifyResult("Check")
                {
                    Count = 0,
                    Message = "曾被生產過",
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
