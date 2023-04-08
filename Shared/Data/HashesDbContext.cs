using Microsoft.EntityFrameworkCore;

namespace Shared.Data;
public class HashesDbContext: DbContext
{
    public HashesDbContext(DbContextOptions<HashesDbContext> dbContextOptions) : base(dbContextOptions) { }

    protected void OnModelCreated(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        //modelBuilder.Entity<Hashes>().HasIndex(e => e.Date);
    }

    public DbSet<Hashes> Hashes { get; set; }
}
