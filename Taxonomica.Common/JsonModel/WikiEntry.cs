using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Taxonomica.Common
{
    [JsonObject(MemberSerialization = MemberSerialization.OptOut)]

    public class WikiEntry
    {
        [JsonProperty("query")]
        public WikiEntryQuery Query { get; set; }

        public string GetThumbnail()
        {
            return Query.Pages.FirstOrDefault().Value.Thumbnail.Source;
        }

        [JsonObject(MemberSerialization = MemberSerialization.OptOut)]
        public class WikiEntryPage
        {
            [JsonProperty("thumbnail")]
            public WikiEntryThumbnail Thumbnail { get; set; }
        }

        [JsonObject(MemberSerialization = MemberSerialization.OptOut)]
        public class WikiEntryQuery
        {
            [JsonProperty("pages")]
            public Dictionary<string, WikiEntryPage> Pages { get; set; }
        }

        [JsonObject(MemberSerialization = MemberSerialization.OptOut)]
        public class WikiEntryThumbnail
        {
            [JsonProperty("height")]
            public string Height { get; set; }

            [JsonProperty("source")]
            public string Source { get; set; }

            [JsonProperty("width")]
            public string Width { get; set; }
        }
    }
}