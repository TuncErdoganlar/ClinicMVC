using APP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    [Authorize] // The cohort is kept per authenticated user.
    public class PatientSessionController : Controller
    {
        private readonly PatientSessionService _patientSessionService;

        public PatientSessionController(PatientSessionService patientSessionService)
        {
            _patientSessionService = patientSessionService;
        }

        // GET: PatientSession
        public IActionResult Index()
        {
            ViewBag.Summary = _patientSessionService.GetSummary();
            return View(_patientSessionService.Get());
        }

        // GET: PatientSession/Add?patientId=5
        public IActionResult Add(int patientId)
        {
            _patientSessionService.Add(patientId);
            TempData["Message"] = "Patient added to the cohort.";
            return RedirectToAction("Index", "Patients");
        }

        // GET: PatientSession/Remove?patientId=5
        public IActionResult Remove(int patientId)
        {
            _patientSessionService.Remove(patientId);
            TempData["Message"] = "Patient removed from the cohort.";
            return RedirectToAction(nameof(Index));
        }

        // GET: PatientSession/Clear
        public IActionResult Clear()
        {
            _patientSessionService.Clear();
            TempData["Message"] = "Cohort cleared.";
            return RedirectToAction(nameof(Index));
        }
    }
}