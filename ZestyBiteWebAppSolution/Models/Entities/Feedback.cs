namespace ZestyBiteWebAppSolution.Models.Entities;

public partial class Feedback {
    public int FbId { get; set; }
    public string FbContent { get; set; } = null!;
    public DateTime FbDatetime { get; set; }
    public string Username { get; set; } = null!;
    public int ItemId { get; set; }
    public int? ParentFbFlag { get; set; }
    // Navigation Properties
    public virtual ICollection<Feedback> InverseParentFbFlagNavigation { get; set; } = new List<Feedback>();
    public virtual Item Item { get; set; } = null!;
    public virtual Feedback? ParentFbFlagNavigation { get; set; }
    public virtual Account UsernameNavigation { get; set; } = null!;
        }
