using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.EFORM;
using Test.Model;

namespace Test.DAO.Interface
{
    public interface IProductRepository
    {
        string GetProductCount(int id);
        Task<bool> CheckOrder(int id);
        Task<Order> GetOrder(int id);
        Task<bool> CheckProduct(string productname);
        IQueryable<EF_Product> GetProducts();
        Task<RS_ModifyResult> CreateProduct(EF_Product DataEntry);
        Task<RS_ModifyResult> UpdateProduct(EF_Product DataEntry);
        Task<RS_ModifyResult> DeleteProduct(EF_Product Id);
    }
}
