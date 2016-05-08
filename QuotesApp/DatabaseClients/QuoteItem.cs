using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesApp.DatabaseClients
{
    public class QuoteItem
    {
        public string Id { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string EMail { get; set; }

        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        [JsonProperty(PropertyName = "highscore")]
        public int Highscore { get; set; }
    }
}
