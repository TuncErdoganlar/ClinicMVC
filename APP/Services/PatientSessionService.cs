using APP.Models;
using CORE.Services;
using CORE.Session.Services;

namespace APP.Services
{
    // Keeps a temporary study cohort (a list of patients) in the session
    // for each authenticated user, and calculates cohort statistics.
    public class PatientSessionService
    {
        const string KEY = "PatientSession";

        private readonly SessionService _sessionService;
        private readonly IService<PatientRequest, PatientResponse> _patientService;

        public PatientSessionService(SessionService sessionService,
            IService<PatientRequest, PatientResponse> patientService)
        {
            _sessionService = sessionService;
            _patientService = patientService;
        }

        // All cohort items in the session (for all users), never null.
        private List<PatientSessionResponse> GetAll()
        {
            return _sessionService.Get<List<PatientSessionResponse>>(KEY)
                ?? new List<PatientSessionResponse>();
        }

        // Cohort items of the authenticated user only.
        public List<PatientSessionResponse> Get()
        {
            return GetAll()
                .Where(ps => ps.AuthenticatedUserId == _sessionService.AuthenticatedUserId)
                .ToList();
        }

        // Cohort statistics as HTML text, empty if the cohort is empty.
        public string GetSummary()
        {
            var cohort = Get();
            if (cohort.Count == 0)
                return string.Empty;
            var summary = $"<b>Patients:</b> {cohort.Count}, " +
                $"<b>Average Age:</b> {cohort.Average(ps => ps.Age):N1}";
            var bmiValues = cohort.Where(ps => ps.Bmi.HasValue).Select(ps => ps.Bmi.Value).ToList();
            if (bmiValues.Count > 0)
                summary += $", <b>Average BMI:</b> {bmiValues.Average():N1}";
            return summary;
        }

        public void Add(int patientId)
        {
            var patient = _patientService.Single(patientId);
            if (patient is null)
                return;
            var all = GetAll();
            if (all.Any(ps => ps.PatientId == patient.Id
                && ps.AuthenticatedUserId == _sessionService.AuthenticatedUserId))
                return;
            all.Add(new PatientSessionResponse
            {
                AuthenticatedUserId = _sessionService.AuthenticatedUserId,
                PatientId = patient.Id,
                FullName = patient.FullNameR,
                Age = patient.AgeR,
                Bmi = patient.Bmi,
                BmiText = patient.BmiR
            });
            _sessionService.Set(KEY, all);
        }

        public void Remove(int patientId)
        {
            var all = GetAll();
            all.RemoveAll(ps => ps.PatientId == patientId
                && ps.AuthenticatedUserId == _sessionService.AuthenticatedUserId);
            _sessionService.Set(KEY, all);
        }

        public void Clear()
        {
            var all = GetAll();
            all.RemoveAll(ps => ps.AuthenticatedUserId == _sessionService.AuthenticatedUserId);
            _sessionService.Set(KEY, all);
        }
    }
}