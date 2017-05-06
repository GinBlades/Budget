using Budget.Domain.Helpers;
using Budget.Domain.Interfaces;
using Budget.Domain.Models;
using Budget.Domain.Models.FormObjects;
using Budget.Domain.SearchTools;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Budget.Domain.Repos
{
    // Alias for laziness
    using ExpFunc = Expression<Func<Entry, bool>>;

    public class EntryRepo : IRepo<Entry, EntrySearchFormObject>
    {
        private readonly ApplicationDbContext _context;
        private readonly RepoHelper<Entry> _helper;
        private readonly DbSet<Entry> _dbSet;
        private readonly ISearchTool<Entry, EntrySearchFormObject> _searchTool;

        public EntryRepo(ApplicationDbContext context, RepoHelper<Entry> helper, ISearchTool<Entry, EntrySearchFormObject> searchTool)
        {
            _context = context;
            _helper = helper;
            _dbSet = _context.Entries;
            _searchTool = searchTool;
        }

        public async Task<List<Entry>> GetList()
        {
            // Return all entries
            return await GetList(_ => 1 == 1);
        }

        public async Task<List<Entry>> GetList(ExpFunc exp)
        {
            return await _dbSet.Include(e => e.User).Where(exp).ToListAsync();
        }

        public async Task<List<Entry>> GetList(EntrySearchFormObject searchForm)
        {
            return await _searchTool.SearchResults(searchForm);
        }

        private IQueryable<Entry> OrderHelper(IQueryable<Entry> query, Expression<Func<Entry, string>> exp, string direction)
        {
            return direction.ToLower() == "asc" ? query.OrderBy(exp) : query.OrderByDescending(exp);
        }

        public async Task<Entry> GetOne(ExpFunc exp)
        {
            return await _dbSet.Include(e => e.User).SingleOrDefaultAsync(exp);
        }

        public async Task<Entry> Create<TFormObject>(TFormObject formObject, params string[] fields)
        {
            var entry = new Entry();
            _helper.StrongUpdate(entry, formObject, fields);
            _dbSet.Add(entry);
            await _context.SaveChangesAsync();
            return entry;
        }

        public async Task<Entry> Update<TFormObject>(int id, TFormObject formObject, params string[] fields)
        {
            var entry = await _context.Entries.FindAsync(id);
            if (entry == null)
            {
                throw new Exception($"Entry with ID {id} not found");
            }
            _helper.StrongUpdate(entry, formObject, fields);
            _context.Entry(entry).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entry;
        }

        public async Task Delete(int id)
        {
            await Delete(await _dbSet.FindAsync(id));
        }

        public async Task Delete(Entry entry)
        {
            if (entry == null)
            {
                return;
            }
            _dbSet.Remove(entry);
            await _context.SaveChangesAsync();
        }
    }
}
