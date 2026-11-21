using CORE.Domain;
using System.ComponentModel;

namespace APP.Models
{
    public class DoctorResponse : Record
    {
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        public bool IsExpert { get; set; }
        public int BranchId { get; set; }
        public int? UserId { get; set; }

        [DisplayName("Doctor")]
        public string FullNameR { get; set; }

        [DisplayName("Expert")]
        public string IsExpertR { get; set; }

        [DisplayName("Branch")]
        public string BranchTitleR { get; set; }

        [DisplayName("User Account")]
        public string UserNameR { get; set; }

        [DisplayName("Patient Count")]
        public int PatientCountR { get; set; }

        [DisplayName("Patients")]
        public string PatientNamesR { get; set; }
    }
}