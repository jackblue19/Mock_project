using ZestyBiteWebAppSolution.Models.ViewModel;

namespace ZestyBiteWebAppSolution.Services.Interfaces {
    public interface IVnPayService {
        string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model);
        VnPaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}
