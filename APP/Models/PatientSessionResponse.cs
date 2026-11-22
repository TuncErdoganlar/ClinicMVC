using System.ComponentModel;

namespace APP.Models
{
    // A patient added to the temporary study cohort kept in the session.
    public class PatientSessionResponse
    {
        public int AuthenticatedUserId { get; set; }

        public int PatientId { get; set; }

        [DisplayName("Patient")]
        public string FullName { get; set; }

        [DisplayName("Age")]
        public int Age { get; set; }

        public decimal? Bmi { get; set; }

        [DisplayName("BMI")]
        public string BmiText { get; set; }
    }
}