using System.Net;
using System.Net.Mail;
using ZestyBiteWebAppSolution.Services.Interfaces;

namespace ZestyBiteWebAppSolution.Services.Implementations {
    public class EmailService : IEmailService {
        private readonly string _smtpServer = "smtp.gmail.com";
        private readonly int _smtpPort = 587;
        private readonly string _smtpUsername = "hn989422@gmail.com";
        private readonly string _smtpPassword = "mloi uxgt wuos dtsd";

        public async Task SendEmailAsync(string email, string subject, string message) {
            using (var client = new SmtpClient(_smtpServer, _smtpPort)) {
                client.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
                client.EnableSsl = true;

                var mailMessage = new MailMessage {
                    From = new MailAddress(_smtpUsername),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(email);

                await client.SendMailAsync(mailMessage);
            }
        }
    }
}
