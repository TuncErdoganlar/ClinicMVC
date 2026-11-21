using CORE.Domain;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APP.Models
{
    public class DoctorRequest : Record
    {
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(50, ErrorMessage = "{0} must be maximum {1} characters!")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(50, ErrorMessage = "{0} must be maximum {1} characters!")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Expert")]
        public bool IsExpert { get; set; }

        // Nullable so that the "-- Select --" option can trigger the Required validation.
        [Required(ErrorMessage = "{0} is required!")]
        [DisplayName("Branch")]
        public int? BranchId { get; set; }

        [DisplayName("User Account")]
        public int? UserId { get; set; }
    }
}