namespace ZestyBiteWebAppSolution.Models.Entities;

public partial class Role {
    public sbyte RoleId { get; set; }

    public string RoleDescription { get; set; } = null!;

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}