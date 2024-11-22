namespace ZestyBiteWebAppSolution.Repositories.Interfaces {
    public interface IEmailConfirmationRepository {
        Task SaveConfirmationTokenAsync(string email, string token);
    }

}
