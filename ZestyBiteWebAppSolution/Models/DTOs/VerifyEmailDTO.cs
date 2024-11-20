using System.ComponentModel.DataAnnotations;

namespace ZestyBiteWebAppSolution.Models.DTOs {
    public class VerifyEmailDTO {

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = null!;

        public string VerificationCode { get; set; } =null!;

    }
}
