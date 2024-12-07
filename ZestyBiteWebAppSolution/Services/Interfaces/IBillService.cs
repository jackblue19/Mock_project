namespace ZestyBiteWebAppSolution.Services.Interfaces {
    public interface IBillService {
        Task<decimal?> CalculateTotalCostAsync();
    }

}
