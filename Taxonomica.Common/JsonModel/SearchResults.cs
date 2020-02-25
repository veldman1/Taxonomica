using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using static Taxonomica.Common.JsonModel.CommonName;

namespace Taxonomica.Common.JsonModel
{
    [JsonObject(MemberSerialization = MemberSerialization.OptOut)]
    public class SearchResults
    {
        public SearchResults()
        {
            MatchList = new List<SearchResult>();
        }

        [JsonProperty("anyMatchList")]
        public List<SearchResult> MatchList { get; set; }

        [JsonObject(MemberSerialization = MemberSerialization.OptOut)]
        public class SearchResult
        {
            [JsonProperty("commonNameList")]
            public CommonNamesList CommonNamesList { get; set; }

            [JsonProperty("tsn")]
            public string TSN { get; set; }

            [JsonProperty("sciName")]
            public string ScientificName { get; set; }

            [JsonIgnore]
            public string FirstCommonName { get; set; }

            public string GetCommonName()
            {
                CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
                TextInfo textInfo = cultureInfo.TextInfo;
                var commonName = CommonNamesList.CommonNames.Where(x => x != null)
                    .FirstOrDefault()?.Name ?? string.Empty;
                return textInfo.ToTitleCase(commonName);
            }
        }
    }
}
