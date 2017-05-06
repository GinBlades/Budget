using Budget.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Budget.Domain.Repos
{
    /// <summary>
    /// User management should be handled with the ASP.NET Identity implementations,
    /// this repository should just be for querying
    /// </summary>
    public class UserRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<ApplicationUser> _dbSet;

        public UserRepo(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Users;
        }

        public async Task<List<ApplicationUser>> GetList()
        {
            return await GetList(_ => 1 == 1);
        }

        public async Task<List<ApplicationUser>> GetList(Expression<Func<ApplicationUser, bool>> exp)
        {
            return await _dbSet.Where(exp).ToListAsync();
        }

        public async Task<ApplicationUser> GetOne(Expression<Func<ApplicationUser, bool>> exp)
        {
            return await _dbSet.Include(u => u.Entries).Include(u => u.AllowanceTasks)
                .SingleOrDefaultAsync(exp);
        }
    }
}
