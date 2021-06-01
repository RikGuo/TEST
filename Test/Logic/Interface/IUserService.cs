using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Model.Interface
{
    public interface IUserService
    {
        Task<EF_User> UserIdentityVerification(string user, string pwd);
        Task<RS_Object> CreateUser(EF_User userIdentity);
        Task<RS_Object> UpdateUser(EF_User userIdentity);
    }
}
