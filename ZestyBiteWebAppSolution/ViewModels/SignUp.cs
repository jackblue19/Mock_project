using System.ComponentModel.DataAnnotations;

namespace ZestyBiteWebAppSolution.ViewModels {
    public class SignUp {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Gender { get; set; } // 1 for Male, 0 for Female
    }
}
