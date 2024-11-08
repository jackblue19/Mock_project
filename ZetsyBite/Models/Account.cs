using System;
using System.Collections.Generic;

namespace ZetsyBite.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Address { get; set; } = null!;

    public bool Gender { get; set; }

    public string Email { get; set; } = null!;

    public string VerificationCode { get; set; } = null!;

    public string ProfileImage { get; set; } = null!;

    public sbyte RoleId { get; set; }

    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual Role Role { get; set; } = null!;
}
