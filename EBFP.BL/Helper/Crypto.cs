using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace EBFP.Helper
{
    public static class Crypto
    {
        private static byte[] key = { };
        private static byte[] IV = { 0X12, 0X34, 0X56, 0X78, 0X90, 0XAB, 0XCD, 0XEF };
        private static string hrisKey
        {
            get
            {
                return CurrentUser.Username + "S3CureRiTY";
            }
           
        }
        private static string hrisKeyLocal
        {
            get
            {
                return  "S3CureRiTY";
            }

        }

        public static string DecryptLocal(this string stringToDecrypt)
        {

            try
            {
                byte[] inputByteArray = new byte[stringToDecrypt.Length + 1];
                stringToDecrypt = stringToDecrypt.Replace("ADD", "+");
                key = System.Text.Encoding.UTF8.GetBytes(hrisKeyLocal.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(stringToDecrypt);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                return encoding.GetString(ms.ToArray());
            }
            catch (Exception e)
            {
                return "";
                //return e.Message;
            }
        }

        public static string Decrypt(this string stringToDecrypt)
        {
           
            try
            {
                byte[] inputByteArray = new byte[stringToDecrypt.Length + 1];
                stringToDecrypt = stringToDecrypt.Replace("ADD", "+");
                key = System.Text.Encoding.UTF8.GetBytes(hrisKey.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(stringToDecrypt);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                System.Text.Encoding encoding = System.Text.Encoding.UTF8; 
                return encoding.GetString(ms.ToArray());
            }
            catch (Exception e)
            {
                return "";
                //return e.Message;
            }
        }

        public static string Encrypt(this string stringToEncrypt)
        {
            try
            { 
                key = System.Text.Encoding.UTF8.GetBytes(hrisKey.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock(); 
                string retStr = Convert.ToBase64String(ms.ToArray()).Replace("+", "ADD");
                return retStr;
            }
            catch (Exception e)
            {
                return "";
                //return e.Message;
            }
        }
    }
}
