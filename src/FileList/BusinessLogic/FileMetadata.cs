using JsonTaggerApi.FileList.EFTypes;
using Newtonsoft.Json;

namespace JsonTaggerApi.FileList.BusinessLogic
{
    public static class FileMetadata
    {
        public static string GetJson(IndexedFile? indexedFile, bool formatted = false)
        {
            string json = "{\"tags\":" + JsonConvert.ToString(FileTags.GetSpaceDelimitedTags(indexedFile)) + "}";
            return formatted
                ? Utility.FormatJson(json)
                : json;
        }
    }
}
