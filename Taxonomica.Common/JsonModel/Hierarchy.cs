using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Taxonomica.Common
{
    [JsonObject(MemberSerialization=MemberSerialization.OptOut)]
    public class Hierarchy
    {
        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("class")]
        public string Class { get; set; }

        [JsonProperty("hierarchyList")]
        public List<HierarchyItem> HierarchyList { get; set; }

        [JsonProperty("rankName")]
        public string RankName { get; set; }

        [JsonProperty("sciName")]
        public string SciName { get; set; }

        [JsonProperty("tsn")]
        public string TSN { get; set; }
    }
}
