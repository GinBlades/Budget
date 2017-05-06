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
    public class AllowanceTasksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IRepo<AllowanceTask> _repo;
        private readonly string[] _strongParams = new string[] { "Title", "Description", "Reward", "Days", "UserId" };

        public AllowanceTasksController(ApplicationDbContext context, IRepo<AllowanceTask> repo)
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
            var task = await Find(id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AllowanceTask task)
        {
            if (ModelState.IsValid)
            {
                await _repo.Create(task, _strongParams);
                return RedirectToAction("Index");
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", task.UserId);
            return View(task);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var task = await Find(id);
            if (task == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", task.UserId);
            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AllowanceTask task)
        {
            if (ModelState.IsValid)
            {
                await _repo.Update(id, task, _strongParams);
                return RedirectToAction("Index");
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", task.UserId);
            return View(task);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var task = await Find(id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repo.Delete(id);
            return RedirectToAction("Index");
        }

        private async Task<AllowanceTask> Find(int? id)
        {
            if (id == null)
            {
                return null;
            }
            return await _repo.GetOne(e => e.Id == id);
        }
    }
}
