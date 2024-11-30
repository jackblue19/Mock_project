namespace ZestyBiteWebAppSolution.Services.Interfaces {
    public interface IVerifyService {
        Task SendVerificationCodeAsync(string userEmail, string code);
    }
}
