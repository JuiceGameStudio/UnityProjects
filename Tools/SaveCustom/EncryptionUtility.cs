using System;
using System.Security.Cryptography;
using System.Text;

public static class EncryptionUtility
{
    private static readonly string secretKey = "FautPasTricher"; 

    public static string Encrypt(string text)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(secretKey.PadRight(32));
            aes.IV = new byte[16];
            using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
            {
                byte[] textBytes = Encoding.UTF8.GetBytes(text);
                byte[] encryptedBytes = encryptor.TransformFinalBlock(textBytes, 0, textBytes.Length);
                return Convert.ToBase64String(encryptedBytes);
            }
        }
    }

    public static string Decrypt(string encryptedText)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(secretKey.PadRight(32));
            aes.IV = new byte[16];
            using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
            {
                byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
                byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }
    }
}
