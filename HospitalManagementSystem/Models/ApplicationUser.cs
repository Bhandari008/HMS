using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        public string? FullName { get; set; }
        public int DepartmentId { get; set; }
        

    }
    public class ApplicationViewUser : IdentityUser
    {
        [Required]
        public string? FullName { get; set; }

        public int DepartmentId { get; set; }
        
       
        public string? RoleName { get; set; }

    }
}
