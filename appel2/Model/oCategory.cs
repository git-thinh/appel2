using Newtonsoft.Json;

namespace appel
{
    public class oCategory
    {
        [JsonProperty("parent")]
        public string Parent { get; set; }

        [JsonProperty("level")]
        public int Level { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonIgnore]
        public string Whatever { get; set; }
    }
}
