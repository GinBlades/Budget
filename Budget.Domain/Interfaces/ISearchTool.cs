using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.Domain.Interfaces
{
    /// <summary>
    /// Contract for search tools that are used by the repos.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity that will be searched</typeparam>
    /// <typeparam name="TSearchFormObject">Type of form object used to pass in parameters to the search</typeparam>
    public interface ISearchTool<TEntity, TSearchFormObject>
    {
        Task<List<TEntity>> SearchResults(TSearchFormObject searchForm);
    }
}
