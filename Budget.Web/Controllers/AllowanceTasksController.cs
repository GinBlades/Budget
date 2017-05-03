using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Budget.Domain;
using Budget.Domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace Budget.Web.Controllers
{
    [Authorize]
    public class AllowanceTasksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AllowanceTasksController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: AllowanceTasks
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.AllowanceTasks.Include(a => a.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AllowanceTasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var allowanceTask = await _context.AllowanceTasks
                .Include(a => a.User)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (allowanceTask == null)
            {
                return NotFound();
            }

            return View(allowanceTask);
        }

        // GET: AllowanceTasks/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: AllowanceTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Reward,Days,CreatedAt,UpdatedAt,UserId")] AllowanceTask allowanceTask)
        {
            if (ModelState.IsValid)
            {
                _context.Add(allowanceTask);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", allowanceTask.UserId);
            return View(allowanceTask);
        }

        // GET: AllowanceTasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var allowanceTask = await _context.AllowanceTasks.SingleOrDefaultAsync(m => m.Id == id);
            if (allowanceTask == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", allowanceTask.UserId);
            return View(allowanceTask);
        }

        // POST: AllowanceTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Reward,Days,CreatedAt,UpdatedAt,UserId")] AllowanceTask allowanceTask)
        {
            if (id != allowanceTask.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(allowanceTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AllowanceTaskExists(allowanceTask.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", allowanceTask.UserId);
            return View(allowanceTask);
        }

        // GET: AllowanceTasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var allowanceTask = await _context.AllowanceTasks
                .Include(a => a.User)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (allowanceTask == null)
            {
                return NotFound();
            }

            return View(allowanceTask);
        }

        // POST: AllowanceTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var allowanceTask = await _context.AllowanceTasks.SingleOrDefaultAsync(m => m.Id == id);
            _context.AllowanceTasks.Remove(allowanceTask);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool AllowanceTaskExists(int id)
        {
            return _context.AllowanceTasks.Any(e => e.Id == id);
        }
    }
}
