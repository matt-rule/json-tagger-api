using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace JsonTaggerApi.FileList.EFTypes
{
    public class IndexedFile
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string FilePath { get; set; } = null!;

        public string Source { get; set; } = null!;

        public string AltSource { get; set; } = null!;

        public string UserJson { get; set; } = null!;

        public string OriginalFilePath { get; set; } = null!;

        public string GuidFilePath { get; set; } = null!;

        public int? CachedWidth { get; set; }

        public int? CachedHeight { get; set; }

        public string CachedShortImageStr { get; set; } = null!;

        public ICollection<IndexedFileTagPair> FileTagPairs { get; set; } = null!;
    }
}
