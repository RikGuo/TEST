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
    public class ProductService:IProductService
    {
        private readonly IProductRepository DaoProduct;
        private const int pageSize = 10;
        public ProductService(IProductRepository daoProduct)
        {
            this.DaoProduct = daoProduct;
        }
        private async Task<bool> CheckOrder(EF_Order DataEntry)
        {
            var temp = await this.DaoProduct.GetOrder(DataEntry.Id);
            //判斷是否曾被下過訂單
            return temp.Id == DataEntry.Id ? false : true;
        }
        public async Task<RS_Product> GetProduct(int? page, string ProductName)
        {
            RS_Product result = new RS_Product();
            try
            {
                var pageIndex = page ?? 1;
                var products = this.DaoProduct.GetProducts();

                if (!string.IsNullOrEmpty(ProductName))
                    products = products.Where(m => m.ProductName.Contains(ProductName));
                result.Count = products.Count();
                result.PageSize = pageSize;
                result.ProductPagedlsit = await products.ToPagedListAsync(pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                Nlogger.WriteLog(Nlogger.NType.Error, $"{ex.Message}{ex.InnerException}", ex);
            }
            return result;
        }
        public async Task<RS_Object> CreateProduct(EF_Product DataEntry)
        {
            RS_Object result = new RS_Object();
            try
            {
                var Rs_Modify = await this.CheckProductName(DataEntry.ProductName);                
                if (Rs_Modify.Success)
                {
                    
                    Rs_Modify = await this.DaoProduct.CreateProduct(DataEntry);
                    Rs_Modify.Message = Rs_Modify.Success ? $"成功新增產品資料{Rs_Modify.Count}筆" : Rs_Modify.Message;

                }
                result = Rs_Modify.Transfor("產品");
            }
            catch (Exception ex)
            {
                Nlogger.WriteLog(Nlogger.NType.Error, $"{ex.Message}{ex.InnerException}", ex);
            }
            return result;
        }

        public async Task<RS_Object> UpdateProduct(EF_Product DataEntry)
        {
            RS_Object result = new RS_Object();
            try
            {
                var Rs_Modify = await this.DaoProduct.UpdateProduct(DataEntry);
                Rs_Modify.Message = Rs_Modify.Success ? $"成功更新產品資料{Rs_Modify.Count}筆" : Rs_Modify.Message;
                result = Rs_Modify.Transfor("產品");
                return result;
            }
            catch (Exception ex)
            {
                Nlogger.WriteLog(Nlogger.NType.Error, $"{ex.Message}{ex.InnerException}", ex);
            }
            return result;
        }

        public async Task<RS_Object> DeleteProduct(EF_Product product,EF_Order order)
        {
            RS_Object result = new RS_Object();
            try
            {
                RS_ModifyResult FinData = new RS_ModifyResult("Check")
                {
                    Success = true
                };
                if (await this.CheckOrder(order))
                    FinData = await this.CheckOrderinProduct(product.Id);
                if (FinData.Success)
                {
                    FinData = await this.DaoProduct.DeleteProduct(product);
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
        
        private IEnumerable<int> GetProductNumber(EF_Product number)
        {
            List<int> result = new List<int>();
            string str = "PT";
            int z = int.Parse(number.Id.ToString().Replace(str, "")) + 1;
            str += z.ToString().PadLeft(7, '0');
            for (int i = 0; i < str.Length; i++)
                result.Add(int.Parse(str));
            return result;
        }
        private async Task<RS_ModifyResult> CheckProductName(string Product)
        {
            bool checkProductId = await this.DaoProduct.CheckProduct(Product);
            if (checkProductId)
                return new RS_ModifyResult("Check")
                {
                    Count = 0,
                    Message = "產品名稱已存在",
                    Success = false
                };
            else
                return new RS_ModifyResult("Check")
                {
                    Success = true
                };
        }

        private async Task<RS_ModifyResult> CheckOrderinProduct(int id)
        {
            bool checkid = await this.DaoProduct.CheckOrder(id);
            if (checkid)
                return new RS_ModifyResult("Check")
                {
                    Count = 0,
                    Message = "已下過訂單",
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
