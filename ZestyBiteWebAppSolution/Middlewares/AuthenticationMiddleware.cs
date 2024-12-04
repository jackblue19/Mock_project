using System.Security.Claims;
using ZestyBiteWebAppSolution.Services.Interfaces;

namespace ZestyBiteWebAppSolution.Middlewares {
    public class AuthenticationMiddleware {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next) {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider) {
            using (var scope = serviceProvider.CreateScope()) {
                var accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();

                // Lấy thông tin người dùng từ Session hoặc Cookies
                var username = context.Session.GetString("username") ?? context.Request.Cookies["username"];

                string userRole = "Customer"; // Mặc định là Customer

                if (!string.IsNullOrEmpty(username)) {
                    // Lấy vai trò người dùng từ dịch vụ
                    userRole = await accountService.GetRoleDescByUsn(username) ?? "Customer";
                }

                if (!string.IsNullOrEmpty(username)) {
                    // Tạo ClaimsPrincipal từ thông tin người dùng
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, username),
                        new Claim(ClaimTypes.Role, userRole)
                    };

                    var identity = new ClaimsIdentity(claims, "Cookies");
                    context.User = new ClaimsPrincipal(identity); // Gán ClaimsPrincipal vào HttpContext.User
                }
            }

            await _next(context); // Tiếp tục chuyển tiếp yêu cầu tới middleware tiếp theo
        }
    }
}
