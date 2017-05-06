using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Budget.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Budget.Domain.Models.FormObjects;
using Budget.Domain.Interfaces;
using Budget.Domain.Extensions;

namespace Budget.Domain.SearchTools
{
    /// <summary>
    /// This returns searched and paginated results. Note the form object is coming in from
    /// a GET query parameter, so any field may be manipulated (include page size, which defaults to 20)
    /// </summary>
    public class EntrySearch : ISearchTool<Entry, EntrySearchFormObject>
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Entry> _dbSet;

        public EntrySearch(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Entries;
        }

        public async Task<List<Entry>> SearchResults(EntrySearchFormObject searchForm)
        {
            var query = _dbSet.AsQueryable()
                .CheckField(searchForm.User, e => e.User.UserName.ToLower().Contains(searchForm.User.ToLower()))
                .CheckField(searchForm.User, e => e.User.UserName.ToLower().Contains(searchForm.User.ToLower()))
                .CheckField(searchForm.Payee, e => e.Payee.ToLower().Contains(searchForm.Payee.ToLower()))
                .CheckField(searchForm.Category, e => e.Category.ToLower().Contains(searchForm.Category.ToLower()))
                .CheckField(searchForm.Notes, e => e.Notes.ToLower().Contains(searchForm.Notes.ToLower()))
                .CheckField(searchForm.FromDate, e => e.EntryDate >= searchForm.FromDate)
                .CheckField(searchForm.ToDate, e => e.EntryDate <= searchForm.ToDate);

            if (searchForm.OrderBy != null)
            {
                var direction = searchForm.OrderDirection ?? "asc";
                switch (searchForm.OrderBy)
                {
                    case "User":
                        query.OrderHelper(e => e.User.UserName, direction);
                        break;
                    case "Payee":
                        query.OrderHelper(e => e.Payee, direction);
                        break;
                    case "Category":
                        query.OrderHelper(e => e.Category, direction);
                        break;
                    case "Notes":
                        query.OrderHelper(e => e.Notes, direction);
                        break;
                    case "EntryDate":
                        query.OrderHelper(e => e.EntryDate, direction);
                        break;
                }
            }
            else
            {
                query = query.OrderByDescending(e => e.EntryDate);
            }

            if (searchForm.Page != null)
            {
                query = query.Skip(((int)searchForm.Page - 1) * searchForm.PerPage);
            }
            query = query.Take(searchForm.PerPage);

            return await query.Include(e => e.User).ToListAsync();
        }
    }
}
