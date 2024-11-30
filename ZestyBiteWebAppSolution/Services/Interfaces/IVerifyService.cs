namespace ZestyBiteWebAppSolution.Services.Implementations {
    public interface IVerifyService {
        Task SendVerificationCodeAsync(string userEmail, string code);
    }
}
