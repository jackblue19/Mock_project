using System.ComponentModel.DataAnnotations;

namespace ZestyBiteWebAppSolution.ViewModels {
    public class VerifyEmail {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
