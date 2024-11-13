using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZestyBiteWebAppSolution.Models {
    public class Accounts : IdentityUser {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountId { get; set; } = 0;

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string Email { get; set; }

        public sbyte RoleId { get; set; }

        public virtual Role Role { get; set; } = null!;

        [Required]
        public bool Gender { get; set; }
    }
}
