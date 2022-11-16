namespace SailorNumberGuessingGame.Server.Extensions;

public static class StringExtensions
{
  public static string ToCamelCaseJsonPath(this string path)
  {
    if (String.IsNullOrEmpty(path)) return path;
    var res = path.ToCamelCase();
    string[] paths = res.Split(".");
    if (paths.Length > 1)
    {
      return $"{paths[0]}.{ToCamelCaseJsonPath(String.Join(".", paths.Skip(1)))}";
    }
    return res;
  }

  public static string ToCamelCase(this string value)
  {
    if (String.IsNullOrEmpty(value)) return value;
    return value.Substring(0, 1).ToLower() + value.Substring(1);
  }

  public static string Encrypt(this string value)
  {
    return Helpers.CryptoHelper.EncryptString(value);
  }

  public static string Decrypt(this string value)
  {
    return Helpers.CryptoHelper.DecryptString(value);
  }
}
