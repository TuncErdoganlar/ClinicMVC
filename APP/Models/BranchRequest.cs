using CORE.Domain;
using System.ComponentModel.DataAnnotations;

namespace APP.Models
{
    public class BranchRequest : Record
    {
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(100, ErrorMessage = "{0} must be maximum {1} characters!")]
        public string Title { get; set; }
    }
}