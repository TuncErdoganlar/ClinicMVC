using CORE.Domain;

namespace APP.Domain
{
    // Relational (junction) entity for the doctors-patients many-to-many relationship.
    public class DoctorPatient : Record
    {
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}