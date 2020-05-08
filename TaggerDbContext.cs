using Microsoft.EntityFrameworkCore;
using System.IO;

namespace JsonTaggerApi
{
    public class TaggerDbContext: DbContext
    {
        public DbSet<DbFile> FileRecords { get; set; } = null!;

        public TaggerDbContext(DbContextOptions<TaggerDbContext> options): base(options) {}
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbFile>()
                .HasKey(x => x.Id);
        }
    }
}
