using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Taxonomica.Common.JsonModel
{
    [JsonObject(MemberSerialization = MemberSerialization.OptOut)]
    public class CommonName
    {
        [JsonProperty("commonName")]
        public string Name { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonObject(MemberSerialization = MemberSerialization.OptOut)]
        public class CommonNamesList
        {
            [JsonProperty("commonNames")]
            public List<CommonName> CommonNames { get; set; }
        }
    }
}
