#nullable disable
using APP.Models;
using CORE.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    [Authorize] // Only authenticated users can use this controller's actions.
    public class BranchesController : Controller
    {
        private readonly IService<BranchRequest, BranchResponse> _branchService;

        public BranchesController(IService<BranchRequest, BranchResponse> branchService)
        {
            _branchService = branchService;
        }

        private void SetTempData(string message, string key = "Message")
        {
            TempData[key] = message;
        }

        // GET: Branches
        public IActionResult Index()
        {
            var list = _branchService.List();
            return View(list);
        }

        // GET: Branches/Details/5
        public IActionResult Details(int id)
        {
            var item = _branchService.Single(id);
            return View(item);
        }

        // GET: Branches/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Branches/Create
        [Authorize(Roles = "Admin")]
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(BranchRequest branch)
        {
            if (ModelState.IsValid)
            {
                var response = _branchService.Add(branch);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message);
                    return RedirectToAction(nameof(Details), new { id = response.Id });
                }
                ModelState.AddModelError("", response.Message);
            }
            return View(branch);
        }

        // GET: Branches/Edit/5
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var item = _branchService.Edit(id);
            return View(item);
        }

        // POST: Branches/Edit
        [Authorize(Roles = "Admin")]
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(BranchRequest branch)
        {
            if (ModelState.IsValid)
            {
                var response = _branchService.Update(branch);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message);
                    return RedirectToAction(nameof(Details), new { id = response.Id });
                }
                ModelState.AddModelError("", response.Message);
            }
            return View(branch);
        }

        // GET: Branches/Delete/5
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var item = _branchService.Single(id);
            return View(item);
        }

        // POST: Branches/Delete
        [Authorize(Roles = "Admin")]
        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var response = _branchService.Remove(id);
            SetTempData(response.Message);
            return RedirectToAction(nameof(Index));
        }
    }
}