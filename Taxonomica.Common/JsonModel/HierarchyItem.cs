using Newtonsoft.Json;

namespace Taxonomica.Common
{
    [JsonObject(MemberSerialization = MemberSerialization.OptOut)]
    public class HierarchyItem
    {
        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("class")]
        public string Class { get; set; }

        [JsonProperty("ParentName")]
        public string ParentName { get; set; }

        [JsonProperty("ParentTsn")]
        public string ParentTsn { get; set; }

        [JsonProperty("rankName")]
        public string RankName { get; set; }

        [JsonProperty("taxonName")]
        public string TaxonName { get; set; }

        [JsonProperty("tsn")]
        public string TSN { get; set; }
    }
}