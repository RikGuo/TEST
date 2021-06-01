using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Test.Content;
using Test.DAO.Interface;
using Test.EFORM;
using Test.Model;

namespace Test.DAO
{
    public class UserRepository: IUserRepository
    {
        private readonly TestContext dbcontext;       
        
        public UserRepository(TestContext dbcontext)
        {
            this.dbcontext = dbcontext;            
            
        }
        protected User Transfor(EF_User DataEntry)
        {
            return new User()
            {
                Id=DataEntry.Id,
                UserId = DataEntry.UserId,
                Password = DataEntry.Password,
                UserName = DataEntry.UserName,
                Email = DataEntry.Email,
                Role = DataEntry.Role,
                CreateDate = DataEntry.CreateDate,
                ModifyDate = DataEntry.ModifyDate
            };
        }
        public async Task<bool> CheckUserId(string userid)
        {
            return await this.dbcontext.User.Where(m => m.UserId== userid).AnyAsync();
        }
        public IQueryable<EF_User> GetIdentityName(string IdentityName)
        {
            var ef_linq = this.dbcontext.User.Where(b => b.UserName == IdentityName).Select(b => new EF_User
            {
                Id=b.Id,
                UserId = b.UserId,
                Password = b.Password,
                UserName = b.UserName,
                Email = b.Email,
                Role = b.Role,
            });
            return ef_linq;
        }
        public async Task<EF_User> GetUser(string user, string pwd)
        {
            var result = await this.dbcontext.User.Where(b => b.UserId == user && b.Password == pwd)
               .FirstOrDefaultAsync();
            if (result != null)
            {
                return new EF_User()
                {
                    Id=result.Id,
                    UserId = result.UserId,
                    Password = result.Password,
                    UserName = result.UserName,
                    Email = result.Email,
                    Role = result.Role,
                };
            }
            else
                return null;
        }
        public async Task<string> GetPwd(string id)
        {
            return await this.dbcontext.User.Where(m => m.UserId == id).Select(b => b.Password).FirstOrDefaultAsync();
        }        
        public async Task<EF_User> GetLogin(string name)
        {
            var result = await this.dbcontext.User.Where(b => b.UserName ==name)
               .FirstOrDefaultAsync();
            if (result != null)
            {
                return new EF_User()
                {
                    Id=result.Id,
                    UserId = result.UserId,
                    UserName = result.UserName,
                    Password = result.Password,
                    Role=result.Role,
                    Email=result.Email  
                };
            }
            else
                return null;
        }
        public async Task<RS_ModifyResult> CreateUser(EF_User DataEntry)
        {
            RS_ModifyResult result = new RS_ModifyResult("Add");
            try
            {
                this.dbcontext.User.Add(this.Transfor(DataEntry));
                var SaveResult = await this.dbcontext.SaveChangesAsync();
                result.Count = SaveResult;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Success = false;
                Nlogger.WriteLog(Nlogger.NType.Error, ex.Message, ex);
            }
            return result;
        }
        public async Task<RS_ModifyResult> UpdateUser(EF_User DataEntry)
        {
            RS_ModifyResult result = new RS_ModifyResult("Update");
            try
            {
                var clone = this.Transfor(DataEntry);
                var ef_find = await this.dbcontext.User.FirstAsync(b => b.Id == DataEntry.Id);
                this.dbcontext.Entry(ef_find).CurrentValues.SetValues(clone);
                this.dbcontext.User.Update(ef_find);
                var SaveResult = await this.dbcontext.SaveChangesAsync();
                result.Count = SaveResult;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Success = false;
                Nlogger.WriteLog(Nlogger.NType.Error, ex.Message, ex);
            }
            return result;
        }

    }
}
