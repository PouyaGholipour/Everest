using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Helper
{
    public static class PasswordHash
    {
        public static string Base64EnCode(string plainText)
        {
            var plainTextByts = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextByts);
        }

        public static string Base64DeCode(string base64EnCodedData)
        {
            var base64EnCodedByts = System.Convert.FromBase64String(base64EnCodedData);
            return System.Text.Encoding.UTF8.GetString(base64EnCodedByts);
        }
    }
}
