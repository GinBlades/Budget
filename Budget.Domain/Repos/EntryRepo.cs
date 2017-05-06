using Budget.Domain.Helpers;
using Budget.Domain.Interfaces;
using Budget.Domain.Models;
using Budget.Domain.Models.FormObjects;
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

    public class EntryRepo : ISearchableRepo<Entry, EntrySearchFormObject>
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

        public async Task<Entry> GetOne(ExpFunc exp)
        {
            return await _dbSet.Include(e => e.User).SingleOrDefaultAsync(exp);
        }

        public async Task<Entry> Create<TFormObject>(TFormObject formObject, params string[] fields)
        {
            var entry = _helper.Add(_dbSet, formObject, fields);
            await _context.SaveChangesAsync();
            return entry;
        }

        public async Task<Entry> Update<TFormObject>(int id, TFormObject formObject, params string[] fields)
        {
            var entry = await _helper.Update(_dbSet, id, formObject, fields);
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
            _helper.Remove(_dbSet, entry);
            await _context.SaveChangesAsync();
        }
    }
}
