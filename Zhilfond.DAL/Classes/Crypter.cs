using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace DAL.Classes
{
    class Crypter
    {
        public static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string CalcHash(string inputString)
        {
            return Convert.ToBase64String(GetHash(inputString));
        }

        public static string DecryptRSA(string crypted, string publickey)
        {
            return string.Empty;
        }    
    }
}
