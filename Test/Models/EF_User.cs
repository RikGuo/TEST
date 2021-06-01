using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Model
{
    public class EF_User
    {
        public int Id { get; set; }
        public string UserId { get; set; }        
        public string Password { get; set; }        
        public string UserName { get; set; }      
        public string Email { get; set; }        
        public string Role { get; set; }        
        public DateTime CreateDate { get; set; }        
        public DateTime ModifyDate { get; set; }
    }
}
