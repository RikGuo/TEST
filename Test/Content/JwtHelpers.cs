using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Test.Model;

namespace Test.Content
{
    public class JwtHelpers
    {
        private readonly static int expireMinutes = 30;

        private readonly IConfiguration Configuration;
        private readonly string issue;
        private readonly string signkey;

        public JwtHelpers(IConfiguration configuration)
        {
            this.Configuration = configuration;
            this.issue = Configuration.GetValue<string>("JwtSettings:Issuer");
            this.signkey = Configuration.GetValue<string>("JwtSettings:SignKey");
        }
        public string JwtGenerateToken(EF_User user)
        {

            List<Claim> claims = SetClaims(user);
            var userClaimsIdentity = new ClaimsIdentity(claims);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signkey));
            // 建立一組對稱式加密的金鑰，主要用於 JWT 簽章之用
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            // HmacSha256 有要求必須要大於 128 bits，所以 key 不能太短，至少要 16 字元以上
            // 建立 SecurityTokenDescriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = issue,
                Subject = userClaimsIdentity,
                Expires = DateTime.Now.AddMinutes(expireMinutes),
                SigningCredentials = signingCredentials
            };
            // 產出所需要的 JWT securityToken 物件，並取得序列化後的 Token 結果(字串格式)
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var serializeToken = tokenHandler.WriteToken(securityToken);

            return serializeToken;
        }
        private List<Claim> SetClaims(EF_User user)
        {
            List<Claim> result = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UserName",user.UserName)                
            };
            return result;
        }

    }
}
