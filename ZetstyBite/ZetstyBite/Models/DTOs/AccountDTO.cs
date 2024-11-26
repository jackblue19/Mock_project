namespace ZetstyBite.Models.DTOs
{
    public class AccountDTO
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Address { get; set; } = null!;
        public ulong Gender { get; set; }
        public string? ProfileImg { get; set; } = "defaul null";
        public string? RoleDescription { get; set; }
        public string? VerificationCode { get; set; } = "default code";

    }
}
