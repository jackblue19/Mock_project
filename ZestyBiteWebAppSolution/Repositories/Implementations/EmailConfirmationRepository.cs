using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Repositories.Interfaces;

namespace ZestyBiteWebAppSolution.Repositories.Implementations {
    public class EmailConfirmationRepository : IEmailConfirmationRepository {
        private readonly Dictionary<string, EmailConfirmationDTO> _tokenStore = new Dictionary<string, EmailConfirmationDTO>();

        public async Task SaveConfirmationTokenAsync(string email, string token) {
            var confirmation = new EmailConfirmationDTO {
                Email = email,
                Token = token,
                ExpirationDate = DateTime.UtcNow.AddHours(1)  // Token có hiệu lực trong 1 giờ
            };

            _tokenStore[email] = confirmation;

            // Bạn có thể lưu vào cơ sở dữ liệu thay vì bộ nhớ trong nếu cần.
            await Task.CompletedTask;  // Vì đang sử dụng bộ nhớ nên không cần thực hiện gì thêm
        }

        public string GenerateEmailConfirmationToken(string email) {
            // Tạo token từ GUID và mã hóa thành chuỗi Base64
            var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            return token;
        }

    }
}
