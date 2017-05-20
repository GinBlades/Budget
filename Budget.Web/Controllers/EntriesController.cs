using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Budget.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Budget.Domain.Interfaces;
using Budget.Domain.Models.FormObjects;
using Budget.Domain.Repos;
using System.Linq;
using System.Security.Claims;

namespace Budget.Web.Controllers
{
    [Authorize]
    public class EntriesController : Controller
    {
        private readonly ISearchableRepo<Entry, EntrySearchFormObject> _repo;
        private readonly string[] _strongParams = new string[] { "Payee", "Category", "Notes", "Price", "EntryDate", "UserId" };
        private readonly UserRepo _userRepo;

        public EntriesController(ISearchableRepo<Entry, EntrySearchFormObject> repo, UserRepo userRepo)
        {
            _repo = repo;
            _userRepo = userRepo;
        }

        public async Task<IActionResult> Index(EntrySearchFormObject search)
        {
            var userId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).First().Value;
            ViewBag.User = await _userRepo.GetOne(u => u.Id == userId);
            return View(await _repo.GetList(search));
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

        public async Task<IActionResult> Create()
        {
            ViewData["UserId"] = new SelectList(await _userRepo.GetList(), "Id", "UserName");
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
            ViewData["UserId"] = new SelectList(await _userRepo.GetList(), "Id", "UserName", entry.UserId);
            return View(entry);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var entry = await Find(id);
            if (entry == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(await _userRepo.GetList(), "Id", "UserName", entry.UserId);
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
            ViewData["UserId"] = new SelectList(await _userRepo.GetList(), "Id", "UserName", entry.UserId);
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
