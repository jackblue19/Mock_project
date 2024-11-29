namespace ZestyBiteWebAppSolution.Models.Entities;

<<<<<<< HEAD
public partial class Profit {
=======
namespace ZestyBiteWebAppSolution.Models.Entities;

public partial class Profit
{
>>>>>>> 5a3b472325e4d2d4a3ebe71e13dd739e0034368d
    public DateTime Date { get; set; }

    public int SupplyId { get; set; }

    public int BillId { get; set; }

    public decimal ProfitAmmount { get; set; }

    public virtual Bill Bill { get; set; } = null!;

    public virtual Supply Supply { get; set; } = null!;
}