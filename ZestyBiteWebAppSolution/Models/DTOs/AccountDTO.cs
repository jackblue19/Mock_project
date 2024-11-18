namespace ZestyBiteWebAppSolution.Models.DTOs {
    public class AccountDTO {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public string? ProfileImg { get; set; } = "defaul null";
        public string RoleDescription { get; set; } = null!;

    }
}