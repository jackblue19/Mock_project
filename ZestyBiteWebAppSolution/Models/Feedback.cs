using System;
using System.Collections.Generic;

namespace ZestyBiteWebAppSolution.Models;

public partial class Feedback
{
    public int FbId { get; set; }

    public string FbContent { get; set; } = null!;

    public DateTime FbDatetime { get; set; }

    public int AccountId { get; set; }

    public int ItemId { get; set; }

    public virtual Accounts Account { get; set; } = null!;

    public virtual Item Item { get; set; } = null!;
}
