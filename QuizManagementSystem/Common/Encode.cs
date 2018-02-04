using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace QuizManagementSystem.Common
{
    public static class Encode
    {
        
        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

        public static string StripHTML(string html)
        {
            return Regex.Replace(html, ConstantVariable.htmlTagPattern, string.Empty);
        }
        public static string StripPTag(string html)
        {
            return Regex.Replace(html, ConstantVariable.pTagPattern, string.Empty);
        }

        public static string StripAnswerLabel(string answer, string strPattern)
        {
            return Regex.Replace(answer, strPattern, string.Empty);
        }
    }
}