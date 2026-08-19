using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ShadowStrap.Utilities.Security
{
    public static class CryptoService
    {
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("ShadowStrapGuardKey2024!"); // В реальности ключ должен быть более защищенным
        private static readonly byte[] Iv = Encoding.UTF8.GetBytes("ShadowStrap_IV_!");

        public static void EncryptToFile(string plainText, string filePath)
        {
            using var aes = Aes.Create();
            using var encryptor = aes.CreateEncryptor(Key, Iv);
            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs))
            {
                sw.Write(plainText);
            }
            File.WriteAllBytes(filePath, ms.ToArray());
        }

        public static string DecryptFromFile(string filePath)
        {
            byte[] cipherText = File.ReadAllBytes(filePath);
            using var aes = Aes.Create();
            using var decryptor = aes.CreateDecryptor(Key, Iv);
            using var ms = new MemoryStream(cipherText);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }
    }
}
