using Microsoft.IdentityModel.Tokens;
using ZestyBiteWebAppSolution.Models.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ZestyBiteWebAppSolution.Services.Implementations
{
    public class TokenService
    {
        private readonly string? _secretKey;
        private readonly string? _issuer;
        private readonly string? _audience;

        public TokenService(IConfiguration configuration)
        {
            _secretKey = configuration["JwtSettings:SecretKey"];
            _issuer = configuration["JwtSettings:Issuer"];
            _audience = configuration["JwtSettings:Audience"];
        }

        // Tạo JWT Token -> sửa thành mỗi username -> gọi AccountService => getbyusnasync -> ko cần truyền roles
        // => sử dụng method GenerateJwtToken of gpt ở cuối thay vì method này
        public string GenerateToken(string userName, List<string> roles)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userName),
            new Claim(ClaimTypes.Role, string.Join(",", roles))
        };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _issuer,
                _audience,
                claims,
                expires: DateTime.Now.AddMinutes(2),
                signingCredentials: credentials
            );
            /*  truyền riêng biệt token 
            var token = new JwtSecurityToken(
                issuer: "MyApp", // Người phát hành token
                audience: "MyAppUsers", // Đối tượng nhận token => phân biệt token bên app và web để chia ra 2 hoặc nhiều dạng token hơn cho từng platform
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );
            */

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Xác thực Token
        public ClaimsPrincipal? ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = _issuer,
                ValidAudience = _audience,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey))
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                return principal;
            }
            catch
            {
                return null; // Nếu token không hợp lệ
            }
        }

        public UserInfo GetUserInfoFromToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("Token is null or empty");
            }

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_secretKey);

                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out var validatedToken);

                var userClaims = principal.Identity as ClaimsIdentity;
                var username = userClaims?.FindFirst(ClaimTypes.Name)?.Value;
                var roles = userClaims?.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

                return new UserInfo { UserName = username, Roles = roles };
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Invalid token", ex);
            }
        }


    }
}

/*                  Check hàm ValidateTOken
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtCookieDemo.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IConfiguration _configuration;

        public AuthenticationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Tạo JWT Token
        private string GenerateJwtToken(string username)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["JwtSettings:Issuer"],
                _configuration["JwtSettings:Audience"],
                claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Đăng nhập và lưu JWT vào Cookie
        [HttpPost("login/cookie")]
        public IActionResult LoginWithCookie(string username)
        {
            var token = GenerateJwtToken(username);

            // Lưu JWT vào Cookie
            Response.Cookies.Append("JWT", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now.AddHours(1)
            });

            return Ok(new { Token = token });
        }

        // Xem thông tin người dùng từ Cookie
        [HttpGet("view/profile/cookie")]
        public IActionResult ViewProfileWithCookie()
        {
            var token = Request.Cookies["JWT"];
            if (string.IsNullOrEmpty(token))
                return Unauthorized(new { Message = "No token provided in cookie" });

            var claimsPrincipal = ValidateToken(token);
            return Ok(new { Message = "Profile viewed", User = claimsPrincipal.Identity.Name });
        }

        // Kiểm tra và xác thực JWT
        private ClaimsPrincipal ValidateToken(string token)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
            var handler = new JwtSecurityTokenHandler();

            try
            {
                var principal = handler.ValidateToken(token, new TokenValidationParameters
                {
                    IssuerSigningKey = key,
                    ValidIssuer = _configuration["JwtSettings:Issuer"],
                    ValidAudience = _configuration["JwtSettings:Audience"],
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out var validatedToken);

                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}

*/


/*      Bản kết hợp GenerateToken với GenerateJwtToken
public string GenerateToken(string userName, List<string> roles)
{
    // Tạo các claims cho người dùng
    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, userName), // Tên người dùng
        new Claim(JwtRegisteredClaimNames.Sub, userName), // Subject (chủ thể) là tên người dùng
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // JWT ID để ngăn chặn việc sử dụng lại token
    };

    // Thêm thông tin về quyền (roles)
    claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

    // Lấy secret key từ cấu hình
    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
    var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    // Tạo đối tượng token với các claims, issuer, audience và thời gian hết hạn
    var token = new JwtSecurityToken(
        _configuration["JwtSettings:Issuer"], // Lấy từ cấu hình
        _configuration["JwtSettings:Audience"], // Lấy từ cấu hình
        claims, // Claims chứa thông tin người dùng và quyền
        expires: DateTime.Now.AddHours(1), // Thời gian hết hạn của token
        signingCredentials: signingCredentials // Thông tin ký token
    );

    // Chuyển đổi token thành chuỗi string và trả về
    return new JwtSecurityTokenHandler().WriteToken(token);
}

*/

