using System.Net;
using System.Net.Mail;
using ZestyBiteWebAppSolution.Helpers;
using Microsoft.Extensions.Options;

namespace ZestyBiteWebAppSolution.Services.Implementations
{
    public interface IVerifyService
    {
        Task SendVerificationCodeAsync(string userEmail, string code);
    }
}
