using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace MultiFaceRec
{
    class md5
    {
        public string GetMD5(string inString)
        {
            MD5 md5 = MD5.Create();
            byte[] bResult = md5.ComputeHash(Encoding.ASCII.GetBytes(inString));
            string sResult = BitConverter.ToString(bResult);
            return sResult.Replace("-", "").ToLower();
        }
    }
}
