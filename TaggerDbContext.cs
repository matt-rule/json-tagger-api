using JsonTaggerApi.EfCompatibleTypes;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace JsonTaggerApi
{
    public class TaggerDbContext: DbContext
    {
        public DbSet<IndexedFile> FileRecords { get; set; } = null!;

        public TaggerDbContext(DbContextOptions<TaggerDbContext> options): base(options) {}
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IndexedFile>()
                .HasKey(x => x.Id);
        }
    }
}
