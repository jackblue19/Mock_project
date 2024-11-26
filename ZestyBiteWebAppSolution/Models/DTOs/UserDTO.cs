using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZestyBiteWebAppSolution.Models.DTOs
{
    public class UserInfo
    {
        public string? UserName { get; set; }
        public List<string> Roles { get; set; }

    }
}