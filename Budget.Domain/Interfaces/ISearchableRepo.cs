using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.Domain.Interfaces
{
    public interface ISearchableRepo<TEntity, TSearchForm> : IRepo<TEntity>
        where TEntity : class, IDBModelTS
        where TSearchForm : class
    {
        Task<List<TEntity>> GetList(TSearchForm searchForm);
    }
}
