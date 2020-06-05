using System.ComponentModel.DataAnnotations.Schema;

namespace JsonTaggerApi.FileList.EFTypes
{
    public class IndexedFileTagPair
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Tag { get; set; } = null!;

        public int FileRecordId { get; set; }

        public IndexedFile FileRecord { get; set; } = null!;

        public IndexedFileTagPair() {}

        public IndexedFileTagPair(string tag, int fileId)
        {
            Tag = tag;
            FileRecordId = fileId;
        }
    }
}
