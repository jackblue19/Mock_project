using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZestyBiteWebAppSolution.Models.DTOs
{
    public class VerifyDTO
    {
        public string Code { get; set; } = null!;
    }
}