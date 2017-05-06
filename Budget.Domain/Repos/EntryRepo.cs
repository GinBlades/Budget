using Budget.Domain.Helpers;
using Budget.Domain.Interfaces;
using Budget.Domain.Models;
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

    public class EntryRepo : IRepo<Entry>
    {
        private readonly ApplicationDbContext _context;
        private readonly RepoHelper<Entry> _helper;
        private readonly DbSet<Entry> _dbSet;

        public EntryRepo(ApplicationDbContext context, RepoHelper<Entry> helper)
        {
            _context = context;
            _helper = helper;
            _dbSet = _context.Entries;
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

        public async Task<List<Entry>> GetList(Dictionary<string, object> searchDictionary)
        {
            var query = _dbSet.AsQueryable();
            foreach(var keyValue in searchDictionary)
            {
                switch (keyValue.Key)
                {
                    case "User":
                        var user = keyValue.Value.ToString();
                        query = query.Where(e => e.User.UserName.Contains(user));
                        continue;
                    case "Payee":
                        var payee = keyValue.Value.ToString();
                        query = query.Where(e => e.Payee.Contains(payee));
                        continue;
                    case "Category":
                        var category = keyValue.Value.ToString();
                        query = query.Where(e => e.Category.Contains(category));
                        continue;
                    case "Notes":
                        var notes = keyValue.Value.ToString();
                        query = query.Where(e => e.Notes.Contains(notes));
                        continue;
                    case "FromDate":
                        var fromDate = Convert.ToDateTime(keyValue.Value);
                        query = query.Where(e => e.EntryDate >= fromDate);
                        continue;
                    case "ToDate":
                        var toDate = Convert.ToDateTime(keyValue.Value);
                        query = query.Where(e => e.EntryDate <= toDate);
                        continue;
                }
            }
            return await query.Include(e => e.User).ToListAsync();
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
