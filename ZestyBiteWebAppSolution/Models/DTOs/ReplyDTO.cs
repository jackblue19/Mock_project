using System.ComponentModel.DataAnnotations;

namespace ZestyBiteWebAppSolution.Models.DTOs {
    public class ReplyDTO {
        public int Id { get; set; }

        [Required(ErrorMessage = "Reply content is required.")]
        public string Content { get; set; } = null!;
        public DateTime DateTime { get; set; }
        public int AccountId { get; set; }
        public string Username { get; set; } = null!;
        public string? ProfileImage { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; } = null!;
        public int ParentFb { get; set; }
    }
}
