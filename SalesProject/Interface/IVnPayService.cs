using SalesProject.Models.Domain;
using SalesProject.Models.DTOs.Response;

namespace SalesProject.Interface
{
    public interface IVnPayService
    {
        Task<string> CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
        Task<PaymentResponseModel> PaymentExecuteAsync(IQueryCollection collections);


    }
}
