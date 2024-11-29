using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZestyBiteWebAppSolution.Models.DTOs {
    public class ChangePwdDTO {
        [Required(ErrorMessage = "Old password is required.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long and not exceed 100 characters.")]
        public string OldPassword { get; set; } = null!;

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
}