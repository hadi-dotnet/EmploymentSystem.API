using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Core.Helper
{
    public static class TokenHelper
    {
        public static string Encode(string token)
        {
            var bytes = Encoding.UTF8.GetBytes(token);
            return Convert.ToBase64String(bytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");
        }

        public static string SafeDecode(string encodedToken)
        {
         
                string incoming = encodedToken
                    .Replace("-", "+")
                    .Replace("_", "/");

               
                incoming = incoming.Trim();
                incoming = incoming.Replace("\u200E", "")
                                   .Replace("\u200F", "")
                                   .Replace("\u202A", "")
                                   .Replace("\u202B", "")
                                   .Replace("\u202C", "")
                                   .Replace("\u202D", "")
                                   .Replace("\u202E", "");

              
                switch (incoming.Length % 4)
                {
                    case 2: incoming += "=="; break;
                    case 3: incoming += "="; break;
                }

                var bytes = Convert.FromBase64String(incoming);
                return Encoding.UTF8.GetString(bytes);
           
        }
    }
}
