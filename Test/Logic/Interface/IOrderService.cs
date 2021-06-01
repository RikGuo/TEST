using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Model;

namespace Test.Logic.Interface
{
    public interface IOrderService
    {
        Task<RS_Order> GetOrderInfo(int? page);
        Task<RS_Object> CreateOrder(EF_Order DataEntry, bool sendemail);
        Task<RS_Object> UpdateOrder(EF_Order DataEntry, EF_Item Item, EF_BOM BOM);
        Task<RS_Object> DeleteOrder(int productid);
    }
}
