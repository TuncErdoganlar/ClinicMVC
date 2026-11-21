using CORE.Domain;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace APP.Domain
{
    // A hospital branch (department) such as Cardiology or Oncology.
    // One branch has many doctors (one-to-many).
    public class Branch : Record
    {
        [Required, StringLength(100)]
        public string Title { get; set; }

        // Navigation property for the branch-doctors one-to-many relationship.
        public List<Doctor> Doctors { get; set; } = new();
    }
}