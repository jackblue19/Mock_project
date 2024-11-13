namespace ZestyBiteWebAppSolution.Models;

public partial class Role {
    public sbyte RoleId { get; set; }

    public string RoleDescription { get; set; } = null!;

    public virtual ICollection<Accounts> Accounts { get; set; } = new List<Accounts>();
}
