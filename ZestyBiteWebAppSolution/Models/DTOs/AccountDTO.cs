using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZestyBiteWebAppSolution.Models.DTOs
{
    public class AccountDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(255, ErrorMessage = "Username cannot exceed 255 characters.")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long and not exceed 100 characters.")]
        public string Password { get; set; } = null!;

        [NotMapped]
        [Required(ErrorMessage = "Confirm Password is required.")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password do not match.")]
        public string ConfirmPassword { get; set; } = null!;

        [Required(ErrorMessage = "Full Name is required.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Phone Number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = "Gender is required.")]
        public int Gender { get; set; }

        public string? ProfileImg { get; set; } = "default null";

        public string RoleDescription { get; set; } = null!;
        public string? VerificationCode { get; set; } = "default code";
        //  nếu đúng logic trình tự code thì nên dể cái verifcation code ở cái hàm riêng được lấy code từ bee gmail
        //  => từ đó kết hợp truyền vào bên signup để confirm lại part2 part1 chỉ mới check so sánh trong db
        //  => nếu muốn thực sự tạo ra được lưu vào trong db để chạy => cần enter verification coded
        //  => truyền thêm đối số verifcation code và vứt cái verfication code ở mục DTO này =)))
        //  còn hiện tại nếu muốn làm code dự án cơ bản xong để hiện thị ra, ờ thì ... lơ cái code này luôn đi =))))
        //  gần như là tương tự với verification code thì trong DTO k có cái acccountID cũng del sao cả =)))))))))
        //  hmm có khả năng giữ nguyên verification code ở đây cũng được nhưng bắt buộc cần phải có 1 nơi để mà truyền đối số để verify auth-code =Đ
        //  Phương án chuẩn nhất là chia ra nhiều DTO cho các method cần impl tương ứng =)))
        //  => như SignInDTO, SignUpDTO, GetAccountDTO, ViewProfileDTO, ...
    }

}
