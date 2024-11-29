namespace ZestyBiteWebAppSolution.Models.Entities;

public partial class Role {
    public sbyte RoleId { get; set; }

    public string RoleDescription { get; set; } = null!;

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
<<<<<<< HEAD
}
=======
}
>>>>>>> 5a3b472325e4d2d4a3ebe71e13dd739e0034368d
