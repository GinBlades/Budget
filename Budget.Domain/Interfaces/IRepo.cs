using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Budget.Domain.Interfaces
{
    public interface IRepo<TEntity, TSearchForm>
        where TEntity : class, IDBModelTS
    {
        Task<List<TEntity>> GetList();
        Task<List<TEntity>> GetList(Expression<Func<TEntity, bool>> exp);
        Task<List<TEntity>> GetList(TSearchForm searchForm);
        Task<TEntity> GetOne(Expression<Func<TEntity, bool>> exp);
        Task<TEntity> Create<TFormObject>(TFormObject formObject, params string[] fields);
        Task<TEntity> Update<TFormObject>(int id, TFormObject formObject, params string[] fields);
        Task Delete(int id);
        Task Delete(TEntity entity);
    }
}
