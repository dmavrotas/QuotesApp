using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace QuotesApp.Model
{
    public class DataService : IDataService
    {
        public async Task<DataItem> GetData()
        {
            // Use this to connect to the actual data service
            return await CallTheService();
        }

        private async Task<DataItem> CallTheService()
        {
            var uri = new Uri("http://quotesondesign.com/wp-json/posts?filter[orderby]=rand&filter[posts_per_page]=1",
                UriKind.RelativeOrAbsolute);

            var client = new HttpClient();
            HttpResponseMessage msg = await client.GetAsync(uri);

            try
            {
                var final = await msg.Content.ReadAsStringAsync();
                var temp = JsonConvert.DeserializeObject<DataItem[]>(final);

                if (temp == null) return null;
                if (!(temp is DataItem[])) return null;

                return await Task.FromResult(temp[0]);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}