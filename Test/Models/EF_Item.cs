using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Model
{
    public class EF_Item
    {
        public int Id { get; set; }        
        public string ItemName { get; set; }
        public int CurrentItem { get; set; }        
        public DateTime? CreateDate { get; set; }        
        public DateTime? ModifyDate { get; set; }       
        
    }
}
