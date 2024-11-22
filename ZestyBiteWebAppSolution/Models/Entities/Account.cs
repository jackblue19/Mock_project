using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ZestyBiteWebAppSolution.Models.Entities;

public class Account {
    [Key]
    public int AccountId { get; set; }

    [Required]
    [StringLength(255)]
    [PersonalData]
    public string UserName { get; set; } = null!;

    [Required]
    [PersonalData]
    public string Password { get; set; } = null!;

    [Required]
    [PersonalData]
    public string Name { get; set; } = null!;

    [Required]
    [Phone]
    [PersonalData]
    public string PhoneNumber { get; set; } = null!;

    [Required]
    [PersonalData]
    public string Address { get; set; } = null!;

    [Required]
    [PersonalData]
    public int Gender { get; set; }

    [Required]
    [EmailAddress]
    [PersonalData]
    public string Email { get; set; } = null!;

    public string VerificationCode { get; set; } = "default";

    public string? ProfileImage { get; set; }

    [Required]
    public sbyte RoleId { get; set; }

    // Navigation properties
    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<Table> Tables { get; set; } = new List<Table>();
    public bool IsEmailConfirmed { get; set; }

}
