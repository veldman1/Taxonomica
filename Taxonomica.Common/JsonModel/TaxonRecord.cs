using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;

namespace Taxonomica.Common
{
    [JsonObject(MemberSerialization = MemberSerialization.OptOut)]
    public class TaxonRecord
    {
        [JsonProperty("commonNameList")]
        public TaxonRecordCommonNamesList CommonNamesList { get; set; }

        [JsonProperty("scientificName")]
        public TaxonRecordScientificName ScientificName { get; set; }

        public string GetCommonName()
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;
            var commonName = CommonNamesList.CommonNames.Where(x => x != null).Where(x => x.Language.Equals("English")).FirstOrDefault()?.CommonName ?? string.Empty;
            return textInfo.ToTitleCase(commonName);
        }

        [JsonObject(MemberSerialization = MemberSerialization.OptOut)]
        public class TaxonRecordCommonName
        {
            [JsonProperty("commonName")]
            public string CommonName { get; set; }

            [JsonProperty("language")]
            public string Language { get; set; }
        }

        [JsonObject(MemberSerialization = MemberSerialization.OptOut)]
        public class TaxonRecordCommonNamesList
        {
            [JsonProperty("commonNames")]
            public List<TaxonRecordCommonName> CommonNames { get; set; }
        }

        [JsonObject(MemberSerialization = MemberSerialization.OptOut)]
        public class TaxonRecordScientificName
        {
            [JsonProperty("combinedName")]
            public string CombinedName { get; set; }
        }
    }
}