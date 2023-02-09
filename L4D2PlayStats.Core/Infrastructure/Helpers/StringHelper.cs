using System.Security.Cryptography;
using System.Text;

namespace L4D2PlayStats.Core.Infrastructure.Helpers;

public static class StringHelper
{
    private static readonly Random Random = new();

    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        return new string(Enumerable.Repeat(chars, length).Select(s => s[Random.Next(s.Length)]).ToArray());
    }

    public static string CreateMd5(string input)
    {
        using var md5 = MD5.Create();
        var inputBytes = Encoding.ASCII.GetBytes(input);
        var hashBytes = md5.ComputeHash(inputBytes);

        return Convert.ToHexString(hashBytes);
    }
}