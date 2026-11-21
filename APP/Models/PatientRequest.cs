using APP.Domain;
using CORE.Domain;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APP.Models
{
    public class PatientRequest : Record
    {
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(50, ErrorMessage = "{0} must be maximum {1} characters!")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(50, ErrorMessage = "{0} must be maximum {1} characters!")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        public Genders Gender { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [DisplayName("Birth Date")]
        public DateTime? BirthDate { get; set; }

        [Range(30, 250, ErrorMessage = "{0} must be between {1} and {2}!")]
        [DisplayName("Height (cm)")]
        public decimal? Height { get; set; }

        [Range(1, 400, ErrorMessage = "{0} must be between {1} and {2}!")]
        [DisplayName("Weight (kg)")]
        public decimal? Weight { get; set; }

        [DisplayName("User Account")]
        public int? UserId { get; set; }

        [DisplayName("Doctors")]
        public List<int> DoctorIds { get; set; }
    }
}