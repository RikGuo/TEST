using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Model
{
    public class EF_Product
    {
        public int Id { get; set; }        
        public string ProductName { get; set; }
        public int? ProductPrice { get; set; }        
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
        
    }
}
