using Newtonsoft.Json;

namespace JsonTaggerApi
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Calls JsonConvert.SerializeObject inline.
        /// </summary>
        /// <returns>The object, in JSON format.</returns>
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
