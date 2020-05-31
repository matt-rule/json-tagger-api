using JsonTaggerApi.Model.EntityFrameworkModels;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace JsonTaggerApi
{
    public class TaggerDbContext: DbContext
    {
        public DbSet<IndexedFile> FileRecords { get; set; } = null!;

        public DbSet<IndexedFileTagPair> FileTagPairs { get; set; } = null!;

        public TaggerDbContext(DbContextOptions<TaggerDbContext> options): base(options) {}
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IndexedFile>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<IndexedFileTagPair>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<IndexedFileTagPair>()
                .HasOne<IndexedFile>(x => x.FileRecord)
                .WithMany(x => x.FileTagPairs)
                .HasForeignKey(x => x.FileRecordId);
        }
    }
}
