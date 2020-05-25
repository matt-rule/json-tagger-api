using System.ComponentModel.DataAnnotations.Schema;

namespace JsonTaggerApi.Types.EfCompatible
{
    public class IndexedFileTagPair
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Tag { get; set; } = null!;

        public int FileId { get; set; }

        public IndexedFile FileRecord { get; set; } = null!;

        public IndexedFileTagPair() {}

        public IndexedFileTagPair(string tag, int fileId)
        {
            Tag = tag;
            FileId = fileId;
        }
    }
}
