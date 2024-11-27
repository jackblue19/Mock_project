namespace ZestyBiteWebAppSolution.Models.Entities;

public partial class Feedback {
    public int FbId { get; set; }

    public string FbContent { get; set; } = null!;

    public DateTime FbDatetime { get; set; }

    public int AccountId { get; set; }

    public int ItemId { get; set; }

    public int? ParentFbFlag { get; set; }
    // Navigation properties
    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<Feedback> InverseParentFbFlagNavigation { get; set; } = new List<Feedback>();

    public virtual Item Item { get; set; } = null!;

    public virtual Feedback? ParentFbFlagNavigation { get; set; }
}
