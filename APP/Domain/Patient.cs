using CORE.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APP.Domain
{
    public class Patient : Record
    {
        [Required, StringLength(50)]
        public string FirstName { get; set; }

        [Required, StringLength(50)]
        public string LastName { get; set; }

        public Genders Gender { get; set; }

        public DateTime BirthDate { get; set; }

        // Height in centimeters and weight in kilograms, both optional.
        public decimal? Height { get; set; }
        public decimal? Weight { get; set; }

        // Optional link to the user account of the patient.
        public int? UserId { get; set; }
        public User User { get; set; }

        // Navigation property for the patients-doctors many-to-many relationship.
        public List<DoctorPatient> DoctorPatients { get; set; } = new();

        // Not a table column: gets or sets the related DoctorPatient records by doctor IDs,
        // the same way User.RoleIds manages UserRoles in the guide.
        [NotMapped]
        public List<int> DoctorIds
        {
            get => DoctorPatients.Select(dp => dp.DoctorId).ToList();
            set => DoctorPatients = value.Select(doctorId => new DoctorPatient
            {
                DoctorId = doctorId
            }).ToList();
        }
    }
}