using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Budget.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Budget.Domain.Interfaces;
using Budget.Domain.Repos;

namespace Budget.Web.Controllers
{
    [Authorize]
    public class AllowanceTasksController : Controller
    {
        private readonly IRepo<AllowanceTask> _repo;
        private readonly string[] _strongParams = new string[] { "Title", "Description", "Reward", "Days", "UserId" };
        private readonly UserRepo _userRepo;

        public AllowanceTasksController(IRepo<AllowanceTask> repo, UserRepo userRepo)
        {
            _repo = repo;
            _userRepo = userRepo;
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

        public async Task<IActionResult> Create()
        {
            ViewData["UserId"] = new SelectList(await _userRepo.GetList(), "Id", "UserName");
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
            ViewData["UserId"] = new SelectList(await _userRepo.GetList(), "Id", "UserName", task.UserId);
            return View(task);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var task = await Find(id);
            if (task == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(await _userRepo.GetList(), "Id", "UserName", task.UserId);
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
            ViewData["UserId"] = new SelectList(await _userRepo.GetList(), "Id", "UserName", task.UserId);
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
