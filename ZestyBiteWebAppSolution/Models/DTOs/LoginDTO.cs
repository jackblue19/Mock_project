using System.ComponentModel.DataAnnotations;

namespace ZestyBiteWebAppSolution.Models.DTOs {
    public class LoginDTO {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(255, ErrorMessage = "Username cannot exceed 255 characters.")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long and not exceed 100 characters.")]
        public string Password { get; set; } = null!;

        public bool RememberMe { get; set; }
    }
}
