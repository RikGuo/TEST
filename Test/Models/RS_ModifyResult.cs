using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Model
{
    public class RS_ModifyResult
    {
        public RS_ModifyResult(string _JobType)
        {
            this.JobType = _JobType;
        }
        public string JobType { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public int Count { get; set; }
        public string Key { get; set; }
        public RS_Object Transfor(string Table)
        {
            return new RS_Object
            {
                TableName = Table,
                Message = this.Message,
                ModifyCount = this.Count,
                Status = this.Success,
                Key = this.Key
            };
        }
    }
}
