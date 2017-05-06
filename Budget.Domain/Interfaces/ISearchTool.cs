using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Domain.Interfaces
{
    public interface ISearchTool<TEntity, TSearchFormObject>
    {
        Task<List<TEntity>> SearchResults(TSearchFormObject searchForm);
    }
}
