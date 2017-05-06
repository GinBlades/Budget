using Budget.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Budget.Domain.Helpers
{
    /// <summary>
    /// Adds some common helpers for Entity Framework entities with Timestamps
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class RepoHelper<TEntity>
        where TEntity : class, IDBModelTS, new()
    {
        public TEntity Add<TFormObject>(DbSet<TEntity> dbSet, TFormObject formObject, params string[] fields)
        {
            var entity = new TEntity();
            StrongUpdate(entity, formObject, fields);
            dbSet.Add(entity);
            return entity;
        }

        public async Task<TEntity> Update<TFormObject>(DbSet<TEntity> dbSet, int id, TFormObject formObject, params string[] fields)
        {
            var entity = await dbSet.FindAsync(id);
            if (entity == null)
            {
                throw new Exception($"Task with ID {id} not found");
            }
            StrongUpdate(entity, formObject, fields);
            return entity;
        }

        public void Remove(DbSet<TEntity> dbSet, TEntity entity)
        {
            if (entity == null)
            {
                throw new Exception($"Entity not found, unable to remove.");
            }

            dbSet.Remove(entity);
        }

        /// <summary>
        /// Check if updating or creating based on Id presence
        /// </summary>
        /// <param name="model">Timestamped Model with ID</param>
        /// <returns>True if creating, false if updating</returns>
        public bool TimeStamp(TEntity model)
        {
            bool creating = false;
            if (model.Id == default(int))
            {
                model.CreatedAt = DateTime.Now;
                creating = true;
            }
            model.UpdatedAt = DateTime.Now;
            return creating;
        }

        /// <summary>
        /// Inspired by Rails 'strong_parameters'
        /// Restricts settable fields to a whitelist.
        /// </summary>
        /// <typeparam name="TFormObject">Form Object containing properties to set on the entity</typeparam>
        /// <param name="model">New or existing model from the database</param>
        /// <param name="formObject">Form object with properties to set</param>
        /// <param name="fields">Whitelist of settable properties</param>
        public void StrongUpdate<TFormObject>(TEntity model, TFormObject formObject, params string[] fields)
        {
            var modelType = model.GetType();
            var foType = formObject.GetType();
            foreach (var field in fields)
            {
                PropertyInfo modelProperty = modelType.GetProperty(field);
                PropertyInfo foProperty = foType.GetProperty(field);
                // If property is not available on the class, PropertyInfo will be null
                if (modelProperty == null || foProperty == null)
                {
                    continue;
                }

                modelProperty.SetValue(model, foProperty.GetValue(formObject));
            }
            TimeStamp(model);
        }
    }
}
