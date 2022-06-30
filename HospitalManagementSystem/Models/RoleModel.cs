using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Models
{
    public class RoleModel
    {
        [Required]
        public string? UserId { get; set; }
        [Required]
        public int RoleId { get; set; }
    }
}
