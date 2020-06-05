using System.ComponentModel.DataAnnotations.Schema;

namespace JsonTaggerApi.Model.EntityFrameworkModels
{
    public class ImageMatch
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Filename1 { get; set; }
        public string Filename2 { get; set; }
        public int Difference { get; set; }

        public ImageMatchDecision Decision { get; set; }

        // This is or was used as a unique identifier for the match itself before the Id column was added.
        public string Guid { get; set; }

        public ImageMatch(string filename1, string filename2, int difference, ImageMatchDecision decision, string guid)
        {
            Filename1 = filename1;
            Filename2 = filename2;
            Difference = difference;
            Decision = decision;
            Guid = guid;
        }

        public static ImageMatch AddGuidToMatchIfMissing(ImageMatch original) =>
            new ImageMatch(
                original.Filename1,
                original.Filename2,
                original.Difference,
                original.Decision,
                original.Guid ?? System.Guid.NewGuid().ToString()
            );
    }
}