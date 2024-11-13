using System.ComponentModel.DataAnnotations;
using ZestyBiteWebAppSolution.Models;

namespace ZestyBiteWebAppSolution.DTO {
    public class AccountDTO {
        public int AccountId { get; set; } = 0;

        public string Username { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public bool Gender { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập email.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; }
        public string? ProfileImage { get; set; }
        public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

        public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    }
}
