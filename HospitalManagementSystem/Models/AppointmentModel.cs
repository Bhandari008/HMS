using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Models
{
    public class AppointmentModel
    {
        public int Id { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public DateTime Schedule { get; set; }
        [Required]
        public int DepartmentId { get; set; }

        public bool isActive { get; set; }

        public string? UserId { get; set; }
        public string? DoctorId { get; set; }
        public string? Condition { get; set; }
    }

    public class AppointmentViewModel
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public DateTime Schedule { get; set; }

        public int DepartmentId { get; set; }


        public string? UserId { get; set; }
        public string? DoctorId { get; set; }
       
        public string? DepartmentName { get; set; }

        public string? Condition { get; set; }
    }
}
