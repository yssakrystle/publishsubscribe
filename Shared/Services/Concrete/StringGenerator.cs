using System.Security.Cryptography;
using System.Text;
using Shared.Services.Contract;

namespace Shared.Services.Concrete;

public class StringGenerator : IStringGenerator
{

    public Stack<string> GenerateRandomString(int numElements, int length, string charSet)
    {
        Stack<string> stack = new();
        for (int x = 0; x <= numElements; x++)
        {
            string source = GenerateRandomString(length, charSet);
            using (SHA1 sha1Hash = SHA1.Create())
            {
                byte[] sourceBytes = Encoding.UTF8.GetBytes(source);
                byte[] hashBytes = sha1Hash.ComputeHash(sourceBytes);
                string hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
                stack.Push(hash);
            }
        }
        return stack;
    }

    private string GenerateRandomString(int length, string charSet)
    {
        var charArray = charSet.Distinct().ToArray();
        char[] result = new char[length];
        for (int i = 0; i < length; i++)
            result[i] = charArray[RandomNumberGenerator.GetInt32(charArray.Length)];
        return new string(result);
    }
}
