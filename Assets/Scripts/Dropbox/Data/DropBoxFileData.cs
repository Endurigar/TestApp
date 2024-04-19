using Newtonsoft.Json;

namespace Dropbox.Data
{
    public class DropBoxFileData
    {
        [JsonProperty(".tag")]
        public string tag { get; set; }
        public string name { get; set; }
        public string path_lower { get; set; }
    }
}
