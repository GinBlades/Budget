using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Budget.Domain.Interfaces;
using Budget.Domain.Models;
using Budget.Domain.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Budget.Domain.Repos
{
    public class TaskRepo : IRepo<AllowanceTask>
    {
        private readonly ApplicationDbContext _context;
        private readonly RepoHelper<AllowanceTask> _helper;
        private readonly DbSet<AllowanceTask> _dbSet;

        public TaskRepo(ApplicationDbContext context, RepoHelper<AllowanceTask> helper)
        {
            _context = context;
            _helper = helper;
            _dbSet = context.AllowanceTasks;
        }

        public async Task<List<AllowanceTask>> GetList()
        {
            return await GetList(_ => 1 == 1);
        }

        public async Task<List<AllowanceTask>> GetList(Expression<Func<AllowanceTask, bool>> exp)
        {
            return await _dbSet.Include(e => e.User).Where(exp).ToListAsync();
        }

        public async Task<AllowanceTask> GetOne(Expression<Func<AllowanceTask, bool>> exp)
        {
            return await _dbSet.Include(e => e.User).SingleOrDefaultAsync(exp);
        }

        public async Task<AllowanceTask> Create<TFormObject>(TFormObject formObject, params string[] fields)
        {
            var task = _helper.Add(_dbSet, formObject, fields);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<AllowanceTask> Update<TFormObject>(int id, TFormObject formObject, params string[] fields)
        {
            var task = await _helper.Update(_dbSet, id, formObject, fields);
            _context.Entry(task).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task Delete(int id)
        {
            await Delete(await _dbSet.FindAsync(id));
        }

        public async Task Delete(AllowanceTask task)
        {
            _helper.Remove(_dbSet, task);
            await _context.SaveChangesAsync();
        }

    }
}
