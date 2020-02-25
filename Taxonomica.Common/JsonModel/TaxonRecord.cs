using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;
using static Taxonomica.Common.JsonModel.CommonName;

namespace Taxonomica.Common
{
    [JsonObject(MemberSerialization = MemberSerialization.OptOut)]
    public class TaxonRecord
    {
        [JsonProperty("commonNameList")]
        public CommonNamesList CommonNamesList { get; set; }

        [JsonProperty("synonymList")]
        public SynonymList SynonymList { get; set; }

        [JsonProperty("scientificName")]
        public ScientificName ScientificName { get; set; }

        [JsonProperty("taxonAuthor")]
        public Author Author { get; set; }

        [JsonProperty("expertList")]
        public ExpertList ExpertList { get; set; }

        [JsonProperty("otherSourceList")]
        public OtherSourcesList OtherSourcesList { get; set; }

        public string GetCommonName()
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;
            var commonName = CommonNamesList.CommonNames.Where(x => x != null).Where(x => x.Language.Equals("English")).FirstOrDefault()?.Name ?? string.Empty;
            return textInfo.ToTitleCase(commonName);
        }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptOut)]
    public class OtherSourcesList
    {
        [JsonProperty("otherSources")]
        public List<OtherSources> OtherSources { get; set; }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptOut)]
    public class OtherSources
    {
        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("sourceComment")]
        public string SourceComment { get; set; }

        [JsonProperty("sourceType")]
        public string SourceType { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("updateDate")]
        public string UpdateDate { get; set; }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptOut)]
    public class ExpertList
    {
        [JsonProperty("experts")]
        public List<Expert> Experts { get; set; }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptOut)]
    public class Expert
    {
        [JsonProperty("expert")]
        public string Name { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("updateDate")]
        public string UpdateDate { get; set; }

        [JsonProperty("referenceFor")]
        public List<ReferenceFor> ReferenceDescription { get; set; }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptOut)]
    public class ReferenceFor
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptOut)]
    public class Synonym
    {
        [JsonProperty("sciName")]
        public string SciName { get; set; }

        [JsonProperty("tsn")]
        public string TSN { get; set; }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptOut)]
    public class SynonymList
    {
        [JsonProperty("synonyms")]
        public List<Synonym> Synonyms { get; set; }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptOut)]
    public class ScientificName
    {
        [JsonProperty("combinedName")]
        public string CombinedName { get; set; }

        [JsonProperty("tsn")]
        public string TSN { get; set; }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptOut)]
    public class Author
    {
        [JsonProperty("authorship")]
        public string Authorship { get; set; }
    }
}