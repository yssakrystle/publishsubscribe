using System.Security.Cryptography;
using Shared.Services.Contract;

namespace Shared.Services.Concrete;

public class StringGenerator : IStringGenerator
{
    public string GenerateRandomString(int length, string charSet)
    {
        var charArray = charSet.Distinct().ToArray();
        char[] result = new char[length];
        for (int i = 0; i < length; i++)
            result[i] = charArray[RandomNumberGenerator.GetInt32(charArray.Length)];
        return new string(result);
    }
}
