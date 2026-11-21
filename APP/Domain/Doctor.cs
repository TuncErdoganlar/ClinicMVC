using CORE.Domain;
using System.ComponentModel.DataAnnotations;

namespace APP.Domain
{
    public class Doctor : Record
    {
        [Required, StringLength(50)]
        public string FirstName { get; set; }

        [Required, StringLength(50)]
        public string LastName { get; set; }

        public bool IsExpert { get; set; }

        // Foreign key and navigation property to the Branch entity (many doctors - one branch).
        public int BranchId { get; set; }
        public Branch Branch { get; set; }

        // Optional link to the user account of the doctor (null if the doctor has no account).
        public int? UserId { get; set; }
        public User User { get; set; }

        // Navigation property for the doctors-patients many-to-many relationship.
        public List<DoctorPatient> DoctorPatients { get; set; } = new();
    }
}