using System.ComponentModel.DataAnnotations;
namespace HospitalManagementSystem.Models;

public class UserModel
{
    [Required]
    public string? Username { get; set; }
    [Required]

    public string FullName { get; set; }

    [Required]
    public string PhoneNumber { get; set; }
    [Required]

    public string? Password { get; set; }
    [Required,Compare("Password",ErrorMessage ="Please enter matching password")]
    public string? ConfirmPassword { get; set; }



}
