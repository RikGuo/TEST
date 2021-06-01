using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.EFORM;
using Test.Model;

namespace Test.DAO.Interface
{
    public interface IItemRepository
    {
        string GetItemCount(int id);
        Task<bool> CheckItem(int ItemId);
        IQueryable<EF_Item> GetItems();
        Task<Item> GetItem(int id);
        Task<RS_ModifyResult> CreateItem(EF_Item DataEntry);
        Task<RS_ModifyResult> UpdateItem(EF_Item DataEntry);
        Task<RS_ModifyResult> DeleteItem(EF_Item Id);
    }
}
