using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Model
{
    public class EF_Order
    {
        public enum OrderStatus
        {
            Established,
            Produced,
            Finished
        }
        public int Id { get; set; }       
        public string OrderSubject { get; set; }        
        public string OrderApplicant { get; set; }
        public string Status { get; set; }        
        public DateTime CreateDate { get; set; }        
        public DateTime ModifyDate { get; set; }
    }
}
