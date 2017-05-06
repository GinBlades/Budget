using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Budget.Domain;
using Budget.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Budget.Domain.Interfaces;

namespace Budget.Web.Controllers
{
    [Authorize]
    public class EntriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IRepo<Entry> _repo;
        private readonly string[] _strongParams = new string[] { "Payee", "Category", "Notes", "Price", "EntryDate", "UserId" };

        public EntriesController(ApplicationDbContext context, IRepo<Entry> repo)
        {
            _context = context;
            _repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _repo.GetList());
        }

        public async Task<IActionResult> Details(int? id)
        {
            var entry = await Find(id);
            if (entry == null)
            {
                return NotFound();
            }

            return View(entry);
        }

        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Entry entry)
        {
            if (ModelState.IsValid)
            {
                await _repo.Create(entry, _strongParams);
                return RedirectToAction("Index");
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", entry.UserId);
            return View(entry);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var entry = await Find(id);
            if (entry == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", entry.UserId);
            return View(entry);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Entry entry)
        {
            if (ModelState.IsValid)
            {
                await _repo.Update(id, entry, _strongParams);
                return RedirectToAction("Index");
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", entry.UserId);
            return View(entry);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var entry = await Find(id);
            if (entry == null)
            {
                return NotFound();
            }

            return View(entry);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repo.Delete(id);
            return RedirectToAction("Index");
        }

        private async Task<Entry> Find(int? id)
        {
            if (id == null)
            {
                return null;
            }
            return await _repo.GetOne(e => e.Id == id);
        }
    }
}
