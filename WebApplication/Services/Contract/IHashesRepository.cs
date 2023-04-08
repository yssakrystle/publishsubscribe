using Shared.Data;
using Shared.Models;

namespace Shared.Services.Contract;

public interface IHashesRepository
{
    Task<List<GetHashesResponse>> GetHashesAsync();
}
