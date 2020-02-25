using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Taxonomica.Common.JsonModel;

namespace Taxonomica.Common
{
    public class RequestManager
    {
        public static async Task<T> Request<T>(string url)
        {
            var httpClient = new HttpClient();
            Uri requestUri = new Uri(url);

            var httpResponse = new HttpResponseMessage();
            var httpResponseBody = string.Empty;

            httpResponse = await httpClient.GetAsync(requestUri);

            httpResponse.EnsureSuccessStatusCode();
            httpResponseBody = await httpResponse.Content.ReadAsStringAsync();

            var response = JsonConvert.DeserializeObject<T>(httpResponseBody);

            return response;
        }

        public static async Task<Hierarchy> RequestFullHierarchy(string tsn)
        {
            var hierarchy = await Request<Hierarchy>("https://www.itis.gov/ITISWebService/jsonservice/getFullHierarchyFromTSN?tsn=" + tsn);
            return hierarchy;
        }

        public static async Task<WikiEntry> RequestWikispeciesImage(string name, int width = 400)
        {
            var wikiEntry = await Request<WikiEntry>("https://species.wikimedia.org/w/api.php?pithumbsize=" + width + "&prop=pageimages&format=json&action=query&titles=" + name);

            return wikiEntry;
        }

        public static async Task<TaxonRecord> RequestFullRecord(string tsn)
        {
            var taxonRecord = await Request<TaxonRecord>("https://www.itis.gov/ITISWebService/jsonservice/getFullRecordFromTSN?tsn=" + tsn);
            return taxonRecord;
        }

        public static async Task<SearchResults> SearchByCommonName(string commonName, int page = 0)
        {
            if (page > 0)
            {
                var taxonRecord = await Request<SearchResults>("https://www.itis.gov/ITISWebService/jsonservice/searchForAnyMatchPaged?srchKey=" + commonName + "&pageSize=25&pageNum=1&ascend=false");
                return taxonRecord;
            }
            else
            {
                var taxonRecord = await Request<SearchResults>("https://www.itis.gov/ITISWebService/jsonservice/searchForAnyMatch?srchKey=" + commonName);
                return taxonRecord;
            }
        }
    }
}
