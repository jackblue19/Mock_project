using System.ComponentModel.DataAnnotations;

namespace ZestyBiteWebAppSolution.Models.DTOs
{
    public class VerifyDTO
    {
        [Required(ErrorMessage = "Verification code is required")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "It should be a 6 digit characters")]
        public string Code { get; set; } = null!;
    }
    public class StatusDTO {

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = null!;
    }
    public class PaymentFlagDTO {
        public string Flag { get; set; }
    }
}