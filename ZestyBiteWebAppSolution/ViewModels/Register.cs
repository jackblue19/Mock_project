using System.ComponentModel.DataAnnotations;

namespace ZestyBiteWebAppSolution.ViewModels {
    public class Register {
        [Required]
        [MaxLength(255)]
        public string Username { get; set; }

        [Required]
        [Phone]
        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        [MaxLength(10)]
        public string Gender { get; set; }


        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [MaxLength(255)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(255)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(255)]
        public string Address { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

    }
}
