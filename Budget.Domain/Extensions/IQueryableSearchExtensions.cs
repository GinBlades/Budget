using System;
using System.Linq;
using System.Linq.Expressions;

namespace Budget.Domain.Extensions
{
    public static class IQueryableSearchExtensions
    {
        public static IQueryable<TEntity> CheckField<TEntity, TField>(this IQueryable<TEntity> query, TField field,
            Expression<Func<TEntity, bool>> exp)
        {
            if (field != null)
            {
                return query.Where(exp);
            }
            return query;
        }

        public static IQueryable<TEntity> OrderHelper<TEntity, TField>(this IQueryable<TEntity> query,
            Expression<Func<TEntity, TField>> exp, string direction)
        {
            return direction.ToLower() == "asc" ? query.OrderBy(exp) : query.OrderByDescending(exp);
        }
    }
}
