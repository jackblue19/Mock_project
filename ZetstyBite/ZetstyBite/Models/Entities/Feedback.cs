using System;
using System.Collections.Generic;

namespace ZetstyBite.Models.Entities;

public partial class Feedback
{
    public int FbId { get; set; }

    public string FbContent { get; set; } = null!;

    public DateTime FbDatetime { get; set; }

    public int AccountId { get; set; }

    public int ItemId { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Item Item { get; set; } = null!;
}
