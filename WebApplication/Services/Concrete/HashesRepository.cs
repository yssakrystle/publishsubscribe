using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Models;
using Shared.Services.Contract;

namespace Shared.Services.Concrete;
public class HashesRepository : IHashesRepository
{
    private readonly HashesDbContext _hashesDbContext;
    public HashesRepository(HashesDbContext hashesDbContext)
    {
        _hashesDbContext = hashesDbContext;
    }

    public async Task<List<GetHashesResponse>> GetHashesAsync()
    {
        return await _hashesDbContext.Hashes
            .GroupBy(x => new { x.Date.Date })
            .Select(x => new GetHashesResponse()
            {
                Date = x.Key.Date,
                Count = x.Count()
            })
            .ToListAsync();
    }
    
}
