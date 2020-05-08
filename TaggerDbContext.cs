using Microsoft.EntityFrameworkCore;
using System.IO;

namespace JsonTaggerApi
{
    public class TaggerDbContext: DbContext
    {
        public DbSet<DbFile> FileRecords { get; set; } = null!;

        public TaggerDbContext(DbContextOptions<TaggerDbContext> options): base(options) {}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dataPath = "/data";
            string dbFileName = "data.db";
            string filePath = Path.Join(dataPath, dbFileName);
            string connectionStr = "Data Source=" + filePath;
            optionsBuilder.UseSqlite(connectionStr);
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbFile>()
                .HasKey(x => x.Id);
        }
    }
}
