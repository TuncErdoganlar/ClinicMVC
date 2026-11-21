using APP.Domain;
using CORE.Domain;
using System.ComponentModel;

namespace APP.Models
{
    public class PatientResponse : Record
    {
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        public Genders Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public decimal? Height { get; set; }
        public decimal? Weight { get; set; }
        public int? UserId { get; set; }

        [DisplayName("Patient")]
        public string FullNameR { get; set; }

        [DisplayName("Gender")]
        public string GenderR { get; set; }

        [DisplayName("Birth Date")]
        public string BirthDateR { get; set; }

        [DisplayName("Age")]
        public int AgeR { get; set; }

        [DisplayName("Height (cm)")]
        public string HeightR { get; set; }

        [DisplayName("Weight (kg)")]
        public string WeightR { get; set; }

        // Body Mass Index = weight (kg) / height (m)^2, null if height or weight is missing.
        public decimal? Bmi { get; set; }

        [DisplayName("BMI")]
        public string BmiR { get; set; }

        [DisplayName("User Account")]
        public string UserNameR { get; set; }

        [DisplayName("Doctors")]
        public string DoctorNamesR { get; set; }

        [DisplayName("Doctors")]
        public List<DoctorResponse> DoctorsR { get; set; }
    }
}