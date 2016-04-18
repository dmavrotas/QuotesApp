using System.Threading.Tasks;
using QuotesApp.Model;

namespace QuotesApp.Design
{
    public class DesignDataService : IDataService
    {
        public Task<DataItem> GetData()
        {
            // Use this to create design time data
            var item = new DataItem("1", "Welcome to MVVM Light [design]", "bla", "link");
            return Task.FromResult(item);
        }
    }
}