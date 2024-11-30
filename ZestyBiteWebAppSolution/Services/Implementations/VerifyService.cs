using System.Net;
using System.Net.Mail;
using ZestyBiteWebAppSolution.Helpers;
using Microsoft.Extensions.Options;
using ZestyBiteWebAppSolution.Services.Interfaces;

namespace ZestyBiteWebAppSolution.Services.Implementations {
    public class VerifySerivce : IVerifyService {
        private readonly EmailSettings _emailSettings;

        public VerifySerivce(IOptions<EmailSettings> emailSettings) {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendVerificationCodeAsync(string userEmail, string code) {
            string? sender = _emailSettings.FromEmail;
            string? senderPwd = _emailSettings.Password;
            string? stmtpServer = _emailSettings.SmtpServer;
            int port = _emailSettings.Port;

            string? reciever = userEmail;
            string emailSubject = "ZestyBite just send you your account verification code";
            string emailBody = $"Your verification code is <b>{code}</b>";

            var smtpClient = new SmtpClient(stmtpServer) {
                Port = port,
                Credentials = new NetworkCredential(sender, senderPwd),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage() {
                From = new MailAddress(sender),
                Subject = emailSubject,
                Body = emailBody,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(reciever);

            smtpClient.Send(mailMessage);
            try {
                await smtpClient.SendMailAsync(mailMessage);
            } catch (Exception ex) {
                throw new InvalidOperationException("Failed to send email.", ex);
            }
        }
    }
}