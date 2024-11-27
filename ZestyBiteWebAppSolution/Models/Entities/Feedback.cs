using System;
using System.Collections.Generic;

namespace ZestyBiteWebAppSolution.Models.Entities
{
    public partial class Feedback
    {
        public int FbId { get; set; } // Primary key
        public string FbContent { get; set; } = null!; // Content of the feedback
        public DateTime FbDatetime { get; set; } // Date and time of the feedback
        public string Username { get; set; } = null!; // Username of the person who gave the feedback
        public int ItemId { get; set; } // Foreign key to the Item
        public int? ParentFbFlag { get; set; } // Foreign key to the parent feedback (nullable)

        // Navigation Properties
        public virtual Account UsernameNavigation { get; set; } = null!; // Navigation to Account
        public virtual ICollection<Feedback> ChildFeedbacks { get; set; } = new List<Feedback>(); // Child feedbacks
        public virtual Item Item { get; set; } = null!; // Navigation to Item
        public virtual Feedback? ParentFbFlagNavigation { get; set; } // Navigation to parent feedback
    }
}