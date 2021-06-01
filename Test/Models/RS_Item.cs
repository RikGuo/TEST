using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace Test.Model
{
    public class RS_Item
    {
        public int Count { get; set; }
        public int PageSize { get; set; }
        public IPagedList<EF_Item> ItemPagedlsit { get; set; }
    }
}
