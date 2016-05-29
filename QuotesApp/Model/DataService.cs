using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using QuotesApp.Converters;
using System.Net;

namespace QuotesApp.Model
{
    public class DataService : IDataService
    {
        #region IDataService Members

        public async Task<DataItem> GetData()
        {
            // Use this to connect to the actual data service
            return await CallTheService();
        }

        public async Task<List<DataItem>> GetWrongAnswersData()
        {
            return await CallTheServiceForFalseData();
        }

        #endregion
        
        #region Private Methods

        private async Task<DataItem> CallTheService()
        {
            var uri = new Uri("http://quotesondesign.com/wp-json/posts?filter[orderby]=rand&filter[posts_per_page]=1",
                UriKind.RelativeOrAbsolute);

            var client = new HttpClient();
            HttpResponseMessage msg = await client.GetAsync(uri);

            try
            {
                var final = await msg.Content.ReadAsStringAsync();
                JsonSerializerSettings settings = new JsonSerializerSettings()
                {
                    StringEscapeHandling = StringEscapeHandling.Default
                };

                var temp = JsonConvert.DeserializeObject<DataItem[]>(final, settings);

                foreach (var item in temp)
                {
                    item.Content = WebUtility.HtmlDecode(item.Content);
                }

                if (temp == null) return null;
                if (!(temp is DataItem[])) return null;
                if (temp.Length == 0) return null;

                return await Task.FromResult(temp[0]);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private async Task<List<DataItem>> CallTheServiceForFalseData()
        {
            var uri = new Uri("http://quotesondesign.com/wp-json/posts?filter[orderby]=rand&filter[posts_per_page]=2",
                UriKind.RelativeOrAbsolute);

            var client = new HttpClient();
            HttpResponseMessage msg = await client.GetAsync(uri);

            try
            {
                var final = await msg.Content.ReadAsStringAsync();
                JsonSerializerSettings settings = new JsonSerializerSettings()
                {
                    StringEscapeHandling = StringEscapeHandling.EscapeHtml,
                };

                var temp = JsonConvert.DeserializeObject<DataItem[]>(final, settings);

                foreach(var item in temp)
                {
                    WebUtility.HtmlDecode(item.Content);
                }

                if (temp == null) return null;
                if (!(temp is DataItem[])) return null;
                if (temp.Length == 0) return null;

                List<DataItem> wrongAnswersData = new List<DataItem>();

                foreach(var item in temp)
                {
                    wrongAnswersData.Add(item);
                }

                return await Task.FromResult(wrongAnswersData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}