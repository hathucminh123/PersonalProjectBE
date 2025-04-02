using SalesProject.Interface;
using SalesProject.Libraries;
using SalesProject.Models.Domain;
using SalesProject.Models.DTOs.Response;
using System.Security.Claims;

namespace SalesProject.Repositories
{
    public class VnPayService: IVnPayService
    {

        private readonly IConfiguration _configuration;

        public VnPayService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreatePaymentUrl(PaymentInformationModel model, HttpContext context)
        {
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var txnRef = Guid.NewGuid().ToString("N").Substring(0, 15); // safer than Ticks
            var pay = new VnPayLibrary();
            var urlCallBack = _configuration["VnPay:PaymentBackReturnUrl"];
            //var urlCallBack = $"{_configuration["PaymentCallBack:ReturnUrl"]}?userId={claim.Id}&amount={model.Amount}";
            if (model.Amount <= 0)
                throw new ArgumentException("Số tiền phải lớn hơn 0");

            pay.AddRequestData("vnp_Version", _configuration["VnPay:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["VnPay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["VnPay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", (model.Amount * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["VnPay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _configuration["VnPay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", $"{model.Name} {model.OrderDescription} {model.Amount}");
            pay.AddRequestData("vnp_OrderType", model.OrderType);
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
            pay.AddRequestData("vnp_TxnRef", txnRef);

            return pay.CreateRequestUrl(_configuration["VnPay:BaseUrl"], _configuration["VnPay:HashSecret"]);
        }

        public PaymentResponseModel PaymentExecute(IQueryCollection collections)
        {
            var pay = new VnPayLibrary();
            return pay.GetFullResponseData(collections, _configuration["VnPay:HashSecret"]);
        }


    }
}
