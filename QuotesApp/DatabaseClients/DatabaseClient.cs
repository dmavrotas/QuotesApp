using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesApp.DatabaseClients
{
    public class DatabaseClient
    {
        #region Members

        private const string ConnectionString = "Server=tcp:quotesapp.database.windows.net,1433;Database=QuotesAppDB;User ID=dmavrotas@quotesapp;Password={your_password_here};Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        #endregion

        #region Constructors

        public DatabaseClient()
        {
            ConnectionString.Replace("{your_password_here}", "Ezio1347@#");
        }

        public void GetClientByEmail(string email)
        {
            //using(var connection =  )
        }

        #endregion
    }
}
