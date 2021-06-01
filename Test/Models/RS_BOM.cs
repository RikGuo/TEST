using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace Test.Model
{
    public class RS_BOM
    {
        public int Count { get; set; }
        public int PageSize { get; set; }
        public IPagedList<EF_BOM> BOMPagedlsit { get; set; }
    }
}
