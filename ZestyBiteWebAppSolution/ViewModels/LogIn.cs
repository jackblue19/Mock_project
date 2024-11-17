using System.ComponentModel.DataAnnotations;

namespace ZestyBiteWebAppSolution.ViewModels {
    public class LogIn {

        [Required]
        [MaxLength(255)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
