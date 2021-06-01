using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.EFORM;
using Test.Model;

namespace Test.DAO.Interface
{
    public interface IUserRepository
    {
        Task<EF_User> GetUser(string user, string pwd);
        Task<bool> CheckUserId(string userid);
        Task<string> GetPwd(string id);
        Task<RS_ModifyResult> CreateUser(EF_User DataEntry);
        Task<RS_ModifyResult> UpdateUser(EF_User DataEntry);
    }
}
