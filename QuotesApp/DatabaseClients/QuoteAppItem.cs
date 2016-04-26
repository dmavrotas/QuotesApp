using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesApp.DatabaseClients
{
    public class QuoteAppItem
    {
        public int Id { get; set; }

        public string EMail { get; set; }

        public string Password { get; set; }

        public int Highscore { get; set; }

        public bool Complete { get; set; }
    }
}
