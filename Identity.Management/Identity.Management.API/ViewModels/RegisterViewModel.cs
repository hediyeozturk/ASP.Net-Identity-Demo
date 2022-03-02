using System.ComponentModel.DataAnnotations;

namespace Identity.Management.API.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; } = "";

        [Required]
        [Compare("Password")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = "";
    }
}
