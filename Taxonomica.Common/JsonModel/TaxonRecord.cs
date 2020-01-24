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

        [JsonProperty("synonymList")]
        public TaxonRecordSynonymList SynonymList { get; set; }

        [JsonProperty("scientificName")]
        public TaxonRecordScientificName ScientificName { get; set; }

        [JsonProperty("taxonAuthor")]
        public TaxonRecordAuthor Author { get; set; }

        public string GetCommonName()
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;
            var commonName = CommonNamesList.CommonNames.Where(x => x != null).Where(x => x.Language.Equals("English")).FirstOrDefault()?.CommonName ?? string.Empty;
            return textInfo.ToTitleCase(commonName);
        }
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
    public class TaxonRecordSynonym
    {
        [JsonProperty("sciName")]
        public string SciName { get; set; }

        [JsonProperty("tsn")]
        public string TSN { get; set; }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptOut)]
    public class TaxonRecordSynonymList
    {
        [JsonProperty("synonyms")]
        public List<TaxonRecordSynonym> Synonyms { get; set; }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptOut)]
    public class TaxonRecordScientificName
    {
        [JsonProperty("combinedName")]
        public string CombinedName { get; set; }

        [JsonProperty("tsn")]
        public string TSN { get; set; }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptOut)]
    public class TaxonRecordAuthor
    {
        [JsonProperty("authorship")]
        public string Author { get; set; }
    }
}