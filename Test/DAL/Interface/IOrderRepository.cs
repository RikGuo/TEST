using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Model;

namespace Test.DAO.Interface
{
    public interface IOrderRepository
    {
        Task<EF_Order> GetStatus(string OrderSubject);
        IQueryable<EF_Order> GetOrders();
        IQueryable<EF_BOM> GetBOMquality(int id);
        IQueryable<EF_Item> GetItemquality(int currentitem);
        Task<bool> CheckItem(int currentitem);        
        Task<RS_ModifyResult> CreateOrder(EF_Order DataEntry);
        Task<RS_ModifyResult> UpdateOrder(EF_Order DataEntry);
        Task<RS_ModifyResult> DeleteOrder(int Id);
    }
}
