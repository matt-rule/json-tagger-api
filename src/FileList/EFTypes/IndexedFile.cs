using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace JsonTaggerApi.FileList.EFTypes
{
    public class IndexedFile
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? FilePath { get; set; }

        public string? Source { get; set; }

        public string? AltSource { get; set; }

        public string? UserJson { get; set; }

        public string? OriginalFilePath { get; set; }

        public string? GuidFilePath { get; set; }

        public int? CachedWidth { get; set; }

        public int? CachedHeight { get; set; }

        public string? CachedShortImageStr { get; set; }

        public ICollection<IndexedFileTagPair> FileTagPairs { get; set; } = null!;
    }
}
