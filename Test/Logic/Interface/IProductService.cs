using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Model;

namespace Test.Logic.Interface
{
    public interface IProductService
    {
        Task<RS_Product> GetProduct(int? page, string ProductName);
        Task<RS_Object> CreateProduct(EF_Product DataEntry);
        Task<RS_Object> UpdateProduct(EF_Product DataEntry);
        Task<RS_Object> DeleteProduct(EF_Product product, EF_Order order);        
    }
}
