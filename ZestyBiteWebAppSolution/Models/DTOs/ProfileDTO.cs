using System.ComponentModel.DataAnnotations;

namespace ZestyBiteWebAppSolution.Models.DTOs {
    public class ProfileDTO {
        // public string Username { get; set; } = null!;
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Phone Number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = "Gender is required.")]
        public int Gender { get; set; }

        public string? ProfileImg { get; set; } = "default null";
    }
}
