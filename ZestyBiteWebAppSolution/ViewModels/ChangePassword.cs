using System.ComponentModel.DataAnnotations;

namespace ZestyBiteWebAppSolution.ViewModels {
    public class ChangePassword {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "ConfirmPassword is required")]
        [DataType(DataType.Password)]
        public string ConfirmNewPassword { get; set; }

        [Required(ErrorMessage = "New Password is required")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "The {0} must be at {2} and at {1} character")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [Compare("ConfirmNewPassword", ErrorMessage = "New Password does not match")]
        public string NewPassword { get; set; }
    }

}
