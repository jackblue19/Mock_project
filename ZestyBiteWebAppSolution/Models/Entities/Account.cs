using System.ComponentModel.DataAnnotations;

namespace ZestyBiteWebAppSolution.Models.Entities;

public class Account {
    [Key]
    public int AccountId { get; set; }

    [Required]
    [StringLength(255)]
    public string UserName { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    [Required]
    public string Name { get; set; } = null!;

    [Required]
    [Phone]
    public string PhoneNumber { get; set; } = null!;

    [Required]
    public string Address { get; set; } = null!;

    [Required]
    public int Gender { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    public string? VerificationCode { get; set; }

    public string? ProfileImage { get; set; }

    [Required]
    public sbyte RoleId { get; set; }

    // Navigation properties
    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<Table> Tables { get; set; } = new List<Table>();
}
