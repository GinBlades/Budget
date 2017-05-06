using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Budget.Domain.Interfaces;
using Budget.Domain.Models;

namespace Budget.Domain.Repos
{
    public class TaskRepo : IRepo<AllowanceTask>
    {
        public Task<AllowanceTask> Create<TFormObject>(TFormObject formObject, params string[] fields)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task Delete(AllowanceTask entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<AllowanceTask>> GetList()
        {
            throw new NotImplementedException();
        }

        public Task<List<AllowanceTask>> GetList(Expression<Func<AllowanceTask, bool>> exp)
        {
            throw new NotImplementedException();
        }

        public Task<List<AllowanceTask>> GetList(Dictionary<string, object> searchDictionary)
        {
            throw new NotImplementedException();
        }

        public Task<AllowanceTask> GetOne(Expression<Func<AllowanceTask, bool>> exp)
        {
            throw new NotImplementedException();
        }

        public Task<AllowanceTask> Update<TFormObject>(int id, TFormObject formObject, params string[] fields)
        {
            throw new NotImplementedException();
        }
    }
}
