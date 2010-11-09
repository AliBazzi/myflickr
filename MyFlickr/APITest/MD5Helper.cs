using System;
using System.Text;
using System.Security.Cryptography;

namespace MD5
{
    public static class MD5Helper
    {
        public static byte[] HashingData(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            return provider.ComputeHash(bytes);
        }

        public static byte[] HashingString(string StringToHash)
        {
            if (string.IsNullOrEmpty(StringToHash))
            {
                throw new ArgumentException("StringToHash");
            }
            return HashingData(UnicodeEncoding.UTF8.GetBytes(StringToHash));
        }

        public static bool ValidateHashEquality(byte[] hashedData1, byte[] hashedData2)
        {
            if (hashedData1 == null)
            {
                throw new ArgumentNullException("hashedData1");
            }
            if (hashedData2 == null)
            {
                throw new ArgumentNullException("hashedData2");
            }
            if (hashedData1.Length != hashedData2.Length)
            {
                return false;
            }
            for (int i = 0; i < hashedData1.Length; i++)
            {
                if (hashedData1[i] != hashedData2[i])
                {
                    return false;
                }
            }
            return true;
        }

        public static string GetHexString(this byte[] hash)
        {
            string ret = "";
            foreach (var byt in hash)
            {
                ret += byt.ToString("x2");
            }
            return ret.ToLower();
        }
    }
}
