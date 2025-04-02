using SalesProject.Models.Domain;
using SalesProject.Models.DTOs.Response;

namespace SalesProject.Interface
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
        PaymentResponseModel PaymentExecute(IQueryCollection collections);

    }
}
