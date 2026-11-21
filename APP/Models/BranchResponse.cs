using CORE.Domain;
using System.ComponentModel;

namespace APP.Models
{
    public class BranchResponse : Record
    {
        public string Title { get; set; }

        [DisplayName("Doctor Count")]
        public int DoctorCountR { get; set; }

        [DisplayName("Doctors")]
        public string DoctorNamesR { get; set; }
    }
}