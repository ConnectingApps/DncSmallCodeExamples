using System.Threading.Tasks;

namespace ConnectingApps.MultiDelegatingHandler
{
    public interface ISearchEngineService
    {
        Task<int> GetNumberOfCharactersFromSearchQuery(string toSearchFor);
    }
}