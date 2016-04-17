using System.Threading.Tasks;

namespace QuotesApp.Model
{
    public interface IDataService
    {
        Task<DataItem> GetData();
    }
}