using System.Security.Cryptography;

namespace SailorNumberGuessingGame.Server.Helpers;

public class CryptoHelper
{
  private static byte[] _key = {
    0x84, 0xe3, 0x14, 0x6c, 0xec, 0x74, 0x23, 0x32,
    0x42, 0x9e, 0x5e, 0x62, 0x02, 0x85, 0x9c, 0xc3,
    0x3e, 0xcb, 0xd6, 0xd7, 0xac, 0xa9, 0x56, 0xf5,
    0x20, 0x15, 0x6b, 0x64, 0x52, 0x12, 0x34, 0x9c
  };

  public static string EncryptString(string plainText)
  {
    Random rnd = new Random();
    byte[] iv = new byte[16];
    rnd.NextBytes(iv);
    return EncryptString(plainText, _key, iv);
  }

  public static string EncryptString(string plainText, byte[] iv)
  {
    return EncryptString(plainText, _key, iv);
  }

  public static string EncryptString(string plainText, byte[] key, byte[] iv)
  {
    byte[] array;
    using (Aes aes = Aes.Create())
    {
      aes.Key = key;
      aes.IV = iv;
      ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
        {
          using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
          {
            streamWriter.Write(plainText);
          }
          array = memoryStream.ToArray();
        }
      }
    }

    byte[] fullArray = new byte[array.Length + 16];
    iv.CopyTo(fullArray, 0);
    array.CopyTo(fullArray, iv.Length);
    return Convert.ToBase64String(fullArray);
  }

  public static string DecryptString(string cipherText)
  {
    return DecryptString(cipherText, _key);
  }
  public static string DecryptString(string cipherText, byte[] key)
  {
    byte[] buffer = Convert.FromBase64String(cipherText);
    byte[] iv = new byte[16];
    Buffer.BlockCopy(buffer, 0, iv, 0, 16);
    byte[] cypherArray = new byte[buffer.Length - 16]; ;
    Buffer.BlockCopy(buffer, 16, cypherArray, 0, cypherArray.Length);
    using (Aes aes = Aes.Create())
    {
      aes.Key = key;
      aes.IV = iv;
      ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
      using (MemoryStream memoryStream = new MemoryStream(cypherArray))
      {
        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
        {
          using (StreamReader streamReader = new StreamReader(cryptoStream))
          {
            return streamReader.ReadToEnd();
          }
        }
      }
    }
  }
}
