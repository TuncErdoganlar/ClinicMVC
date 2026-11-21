#nullable disable
using APP.Models;
using CORE.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC.Controllers
{
    [Authorize]
    public class DoctorsController : Controller
    {
        private readonly IService<DoctorRequest, DoctorResponse> _doctorService;
        private readonly IService<BranchRequest, BranchResponse> _branchService;
        private readonly IService<UserRequest, UserResponse> _userService;

        public DoctorsController(
            IService<DoctorRequest, DoctorResponse> doctorService
            , IService<BranchRequest, BranchResponse> branchService
            , IService<UserRequest, UserResponse> userService
        )
        {
            _doctorService = doctorService;
            _branchService = branchService;
            _userService = userService;
        }

        // Fills the drop down lists of the Create and Edit views.
        private void SetViewData()
        {
            ViewData["BranchId"] = new SelectList(_branchService.List(), "Id", "Title");
            ViewData["UserId"] = new SelectList(_userService.List(), "Id", "UserName");
        }

        private void SetTempData(string message, string key = "Message")
        {
            TempData[key] = message;
        }

        // GET: Doctors
        [AllowAnonymous] // The doctor directory is public: anyone can see the list without logging in.
        public IActionResult Index()
        {
            var list = _doctorService.List();
            return View(list);
        }

        // GET: Doctors/Details/5
        public IActionResult Details(int id)
        {
            var item = _doctorService.Single(id);
            return View(item);
        }

        // GET: Doctors/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            SetViewData();
            return View();
        }

        // POST: Doctors/Create
        [Authorize(Roles = "Admin")]
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(DoctorRequest doctor)
        {
            if (ModelState.IsValid)
            {
                var response = _doctorService.Add(doctor);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message);
                    return RedirectToAction(nameof(Details), new { id = response.Id });
                }
                ModelState.AddModelError("", response.Message);
            }
            SetViewData();
            return View(doctor);
        }

        // GET: Doctors/Edit/5
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var item = _doctorService.Edit(id);
            SetViewData();
            return View(item);
        }

        // POST: Doctors/Edit
        [Authorize(Roles = "Admin")]
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(DoctorRequest doctor)
        {
            if (ModelState.IsValid)
            {
                var response = _doctorService.Update(doctor);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message);
                    return RedirectToAction(nameof(Details), new { id = response.Id });
                }
                ModelState.AddModelError("", response.Message);
            }
            SetViewData();
            return View(doctor);
        }

        // GET: Doctors/Delete/5
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var item = _doctorService.Single(id);
            return View(item);
        }

        // POST: Doctors/Delete
        [Authorize(Roles = "Admin")]
        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var response = _doctorService.Remove(id);
            SetTempData(response.Message);
            return RedirectToAction(nameof(Index));
        }
    }
}