using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Model;

namespace Test.Logic.Interface
{
    public interface IItemService
    {
        Task<RS_Item> GetItem(int? page, string ItemName);
        Task<RS_Object> CreateItem(EF_Item DataEntry);
        Task<RS_Object> UpdateItem(EF_Item DataEntry);
        Task<RS_Object> DeleteItem(EF_Item currentitem);
    }
}
