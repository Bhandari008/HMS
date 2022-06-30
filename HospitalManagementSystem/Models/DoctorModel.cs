using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Models
{
    public class DoctorModel
    {
        public string? Id { get; set; }

        public string? Name { get; set; }
        [Required]
        public int DepartmentId { get; set; }

    }
}
