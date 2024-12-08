using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZestyBiteWebAppSolution.Models.DTOs {
    public class ForgotPwdDTO {
        [Required(ErrorMessage = "Verification code is required")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "It should be a 6 digit characters")]
        public string Code { get; set; } = null!;

        [Required(ErrorMessage = "New password is required.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long and not exceed 100 characters.")]
        public string NewPassword { get; set; } = null!;

        [NotMapped]
        [Required(ErrorMessage = "Confirm New Password is required.")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "New Password and Confirmed New Password do not match.")]
        public string ConfirmNewPassword { get; set; } = null!;
    }
    public class MailDTO {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
    }
}