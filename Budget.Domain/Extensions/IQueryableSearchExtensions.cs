using System;
using System.Linq;
using System.Linq.Expressions;

namespace Budget.Domain.Extensions
{
    /// <summary>
    /// This adds a couple extensions used for searching from the search tools.
    /// </summary>
    public static class IQueryableSearchExtensions
    {
        /// <summary>
        /// Check if field is null before adding the expression to the query
        /// </summary>
        /// <typeparam name="TEntity">Type ofEntity being search</typeparam>
        /// <typeparam name="TField">Type of nullable search field being checked</typeparam>
        /// <param name="query">Current IQueryable chain</param>
        /// <param name="field">Field being checked for null</param>
        /// <param name="exp">Expression to search field with</param>
        /// <returns>Chainable query</returns>
        public static IQueryable<TEntity> CheckField<TEntity, TField>(this IQueryable<TEntity> query, TField field,
            Expression<Func<TEntity, bool>> exp)
        {
            if (field != null)
            {
                return query.Where(exp);
            }
            return query;
        }

        /// <summary>
        /// Orders by ascending or descending based on string value passed ina. Defaults to descending
        /// </summary>
        /// <typeparam name="TEntity">Type ofEntity being search</typeparam>
        /// <typeparam name="TField">Field to order by</typeparam>
        /// <param name="query">Current IQueryable chain</param>
        /// <param name="exp">Expression to set order field with</param>
        /// <param name="direction">Direction to order by</param>
        /// <returns>Chainable query</returns>
        public static IQueryable<TEntity> OrderHelper<TEntity, TField>(this IQueryable<TEntity> query,
            Expression<Func<TEntity, TField>> exp, string direction)
        {
            return direction.ToLower() == "asc" ? query.OrderBy(exp) : query.OrderByDescending(exp);
        }
    }
}
