using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Content
{
    public class SHA1
    {
        public static string ToSHA1(string str)
        {
            using (var cryptoSHA = System.Security.Cryptography.SHA1.Create())
            {
                var bytes = Encoding.Unicode.GetBytes(str);
                var hash = cryptoSHA.ComputeHash(bytes);
                var sha1 = BitConverter.ToString(hash)
                  .Replace("-", String.Empty)
                  .ToUpper();

                return sha1;
            }
        }
    }
}
