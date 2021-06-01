using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Model
{
    public class EF_BOM
    {
        public int Autoid { get; set; }        
        public int IdProduct { get; set; }        
        public int IdItem { get; set; }
        public int ItemNumber { get; set; }        
        public DateTime CreateDate { get; set; }        
        public DateTime ModifyDate { get; set; }
    }
}
