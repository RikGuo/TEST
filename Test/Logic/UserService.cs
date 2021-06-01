using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Content;
using Test.DAO.Interface;
using Test.Model.Interface;

namespace Test.Model
{
    public class UserService : IUserService
    {
        private readonly IUserRepository Daouser;
        public UserService(IUserRepository Daouser)
        {
            this.Daouser = Daouser;
        }
        public async Task<EF_User> UserIdentityVerification(string user, string pwd)
        {
            var Md5Pwd = SHA1.ToSHA1(pwd);
            return await this.Daouser.GetUser(user, Md5Pwd);
        }

        private string SHA1Tranfor(string Pwd)
        {
            return Content.SHA1.ToSHA1(Pwd);
        }
        private async Task<bool> CheckUserNameChange(EF_User DataEntry)
        {
            var temp = await this.Daouser.GetUser(DataEntry.UserId, DataEntry.Password);
            //判斷是否有修改帳號
            return temp.UserName == DataEntry.UserName ? false : true;
        }
        private async Task<string> UserPwd(EF_User DataEntry)
        {
            return await this.Daouser.GetPwd(DataEntry.UserId);
        }
        public async Task<RS_Object> CreateUser(EF_User DataEntry)
        {
            RS_Object result = new RS_Object();
            try
            {
                var Rs_Modify = await this.CheckUserId(DataEntry.UserId);
                if (Rs_Modify.Success)
                {
                    DataEntry.Password = this.SHA1Tranfor(DataEntry.Password);
                    Rs_Modify = await this.Daouser.CreateUser(DataEntry);
                    Rs_Modify.Message = Rs_Modify.Success ? $"成功新增使用者資料{Rs_Modify.Count}筆" : Rs_Modify.Message;

                }
                result = Rs_Modify.Transfor("使用者");
            }
            catch (Exception ex)
            {
                Nlogger.WriteLog(Nlogger.NType.Error, $"{ex.Message}{ex.InnerException}", ex);
            }
            return result;
        }       

        public async Task<RS_Object> UpdateUser(EF_User userIdentity)
        {
            RS_Object rS_Commonality = new RS_Object();
            try
            {
                RS_ModifyResult FinData = new RS_ModifyResult("Check");
                FinData.Success = true;
                if (await this.CheckUserNameChange(userIdentity))
                    FinData = await this.CheckUserId(userIdentity.UserId);
                if (FinData.Success)
                {
                    if (!string.IsNullOrEmpty(userIdentity.Password))
                        userIdentity.Password = this.SHA1Tranfor(userIdentity.Password);
                    else
                        userIdentity.Password = await this.UserPwd(userIdentity);

                    FinData = await this.Daouser.UpdateUser(userIdentity);
                    FinData.Message = $"更新帳號:{userIdentity.UserName} \n結果:{(FinData.Success ? "成功" : "失敗")}";
                }
                rS_Commonality = FinData.Transfor("User");
                Nlogger.WriteLog(Nlogger.NType.Info, FinData.Message);
            }
            catch (Exception ex)
            {
                Nlogger.WriteLog(Nlogger.NType.Error, $"{ex.Message}{ex.InnerException}", ex);
            }
            return rS_Commonality;
        }

        private async Task<RS_ModifyResult> CheckUserId(string User)
        {
            bool checkUserName = await this.Daouser.CheckUserId(User);
            if (checkUserName)
                return new RS_ModifyResult("Check")
                {
                    Count = 0,
                    Message = "帳號已存在",
                    Success = false
                };
            else
                return new RS_ModifyResult("Check")
                {
                    Success = true
                };
        }
    }
}
