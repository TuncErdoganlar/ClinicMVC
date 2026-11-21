#nullable disable
using APP.Models;
using CORE.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC.Controllers
{
    [Authorize] // Patient data is private: only authenticated users can see it.
    public class PatientsController : Controller
    {
        private readonly IService<PatientRequest, PatientResponse> _patientService;
        private readonly IService<DoctorRequest, DoctorResponse> _doctorService;
        private readonly IService<UserRequest, UserResponse> _userService;

        public PatientsController(
            IService<PatientRequest, PatientResponse> patientService
            , IService<DoctorRequest, DoctorResponse> doctorService
            , IService<UserRequest, UserResponse> userService
        )
        {
            _patientService = patientService;
            _doctorService = doctorService;
            _userService = userService;
        }

        private void SetViewData()
        {
            ViewData["UserId"] = new SelectList(_userService.List(), "Id", "UserName");
            // Many-to-many: list box for selecting the patient's doctors.
            ViewBag.DoctorIds = new MultiSelectList(_doctorService.List(), "Id", "FullNameR");
        }

        private void SetTempData(string message, string key = "Message")
        {
            TempData[key] = message;
        }

        // GET: Patients
        public IActionResult Index()
        {
            var list = _patientService.List();
            return View(list);
        }

        // GET: Patients/Details/5
        public IActionResult Details(int id)
        {
            var item = _patientService.Single(id);
            return View(item);
        }

        // GET: Patients/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            SetViewData();
            return View();
        }

        // POST: Patients/Create
        [Authorize(Roles = "Admin")]
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(PatientRequest patient)
        {
            if (ModelState.IsValid)
            {
                var response = _patientService.Add(patient);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message);
                    return RedirectToAction(nameof(Details), new { id = response.Id });
                }
                ModelState.AddModelError("", response.Message);
            }
            SetViewData();
            return View(patient);
        }

        // GET: Patients/Edit/5
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var item = _patientService.Edit(id);
            SetViewData();
            return View(item);
        }

        // POST: Patients/Edit
        [Authorize(Roles = "Admin")]
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(PatientRequest patient)
        {
            if (ModelState.IsValid)
            {
                var response = _patientService.Update(patient);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message);
                    return RedirectToAction(nameof(Details), new { id = response.Id });
                }
                ModelState.AddModelError("", response.Message);
            }
            SetViewData();
            return View(patient);
        }

        // GET: Patients/Delete/5
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var item = _patientService.Single(id);
            return View(item);
        }

        // POST: Patients/Delete
        [Authorize(Roles = "Admin")]
        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var response = _patientService.Remove(id);
            SetTempData(response.Message);
            return RedirectToAction(nameof(Index));
        }
    }
}