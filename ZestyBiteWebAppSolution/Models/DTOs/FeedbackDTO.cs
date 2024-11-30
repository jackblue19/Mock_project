using System;
using System.ComponentModel.DataAnnotations;

namespace ZestyBiteWebAppSolution.Models.DTOs
{
    public class FeedbackDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Feedback content is required.")]
        public string Content { get; set; } = null!;
        public DateTime DateTime { get; set; }
        public string Username { get; set; } = null!;
        public string? ProfileImage { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; } = null!;
        public int? ParentFb { get; set; } = null!;
        //to include parent feedback if needed
        public FeedbackDTO? ParentFeedback { get; set; }
        public bool IsReply { get; set; }
    }
}