namespace Shared.Services.Contract;

public interface IStringGenerator
{
    string GenerateRandomString(int length, string charSet);
}
