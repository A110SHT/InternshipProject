using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogics.Common
{
    public class CommonMethods
    {
        public static string key = "jash2347@!kfj";

        public static string ConvertToEncrypt(string objPassword)
        {
            if (string.IsNullOrEmpty(objPassword))
            {
                return "";
            }
            objPassword += key;
            var passwordBytes = Encoding.UTF8.GetBytes(objPassword);
            return Convert.ToBase64String(passwordBytes);
        }

        public static string ConvertToDecrypt(string base64EncodeData)
        {
            if (string.IsNullOrEmpty(base64EncodeData))
            {
                return "";
            }
            var base64EncodeBytes = Convert.FromBase64String(base64EncodeData);
            var result = Encoding.UTF8.GetString(base64EncodeBytes);
            result = result.Substring(0, result.Length - key.Length);
            return result;

            
        }
    }
}
