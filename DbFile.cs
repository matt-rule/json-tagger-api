using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace json_tagger_api
{
    public class DbFile
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string FilePath { get; set; }

        public string Source { get; set; }

        public string AltSource { get; set; }

        public string UserJson { get; set; }

        public string OriginalFilePath { get; set; }

        public string GuidFilePath { get; set; }

        public int? CachedWidth { get; set; }

        public int? CachedHeight { get; set; }

        public string CachedShortImageStr { get; set; }
    }
}
