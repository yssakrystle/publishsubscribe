namespace Shared.Services.Contract;

public interface IStringGenerator
{
    Stack<string> GenerateRandomString(int numElements, int length, string charSet);
}
