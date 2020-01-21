using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

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
    }
}
