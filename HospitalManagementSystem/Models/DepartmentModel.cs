using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Models
{
    public class DepartmentModel
    {
        public int? Id { get; set; }
        [Required]
        public string? DepartmentName { get; set; }
    }
}
