using Azure;
using Microsoft.EntityFrameworkCore;
using SalesProject.Data;
using SalesProject.Interface;
using SalesProject.Libraries;
using SalesProject.Models.Domain;
using SalesProject.Models.DTOs.Response;
using System.Security.Claims;
using static Google.Apis.Requests.BatchRequest;

namespace SalesProject.Repositories
{
    public class VnPayService: IVnPayService
    {

        private readonly IConfiguration _configuration;
        private readonly SalesDbContext _context;

        public VnPayService(IConfiguration configuration,SalesDbContext salesDbContext)
        {
            _configuration = configuration;
          
            _context = salesDbContext;
        }

        public async Task<string> CreatePaymentUrl(PaymentInformationModel model, HttpContext context)
        {
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);

            //if (model.Amount <= 0)
            //    throw new ArgumentException("Số tiền phải lớn hơn 0");

            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == model.OrderId);
            if (order == null)
                throw new ArgumentException("Không tìm thấy đơn hàng hợp lệ");

            var txnRef = order.Id.ToString("N");

            var pay = new VnPayLibrary();

            pay.AddRequestData("vnp_Version", _configuration["VnPay:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["VnPay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["VnPay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", ((int)(order.TotalPrice * 100)).ToString()); // nhân 100
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["VnPay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _configuration["VnPay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", $"{model.Name} - {model.OrderDescription}");
            pay.AddRequestData("vnp_OrderType", model.OrderType);
            pay.AddRequestData("vnp_ReturnUrl", _configuration["VnPay:PaymentBackReturnUrl"]);
            pay.AddRequestData("vnp_TxnRef", txnRef);

            return pay.CreateRequestUrl(_configuration["VnPay:BaseUrl"], _configuration["VnPay:HashSecret"]);
        }

        public async Task<PaymentResponseModel> PaymentExecuteAsync(IQueryCollection collections)
        {
            var pay = new VnPayLibrary();
            var res = pay.GetFullResponseData(collections, _configuration["VnPay:HashSecret"]);

            if (collections.TryGetValue("vnp_TxnRef", out var txnRefStr)
                && Guid.TryParseExact(txnRefStr, "N", out var orderId))
            {
                var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
                if (order == null)
                    return new PaymentResponseModel { Success = false, Message = "Không tìm thấy đơn hàng." };

                var payment = await _context.Payments.FirstOrDefaultAsync(p => p.OrderId == order.Id);

                // 🔹 Nếu chưa có payment thì tạo mới
                if (payment == null)
                {
                    payment = new Payment
                    {
                        Id = Guid.NewGuid(),
                        OrderId = order.Id,
                        PaymentMethod = PaymentMethodEnum.VNPay,
                        Amount = order.TotalPrice,
                        CreatedAt = DateTime.UtcNow
                    };
                    _context.Payments.Add(payment);
                }

                if (res.Success && res.VnPayResponseCode == "00")
                {
                    order.Status = OrderStatusEnum.Completed;
                    order.CreatedAt = DateTime.UtcNow;
                    order.PaymentStatus = PaymentStatusEnum.Completed;

                    payment.PaymentStatus = PaymentStatusEnum.Completed;
                    payment.TransactionId = res.TransactionId;

                    await _context.SaveChangesAsync();

                    return new PaymentResponseModel
                    {
                        Success = true,
                        Message = "Thanh toán thành công.",
                        TransactionId = res.TransactionId,
                        OrderId = order.Id.ToString()
                    };
                }
                else
                {
                    payment.PaymentStatus = PaymentStatusEnum.Failed;
                    payment.TransactionId = res.TransactionId;

                    await _context.SaveChangesAsync();

                    return new PaymentResponseModel
                    {
                        Success = false,
                        Message = "Thanh toán thất bại hoặc bị hủy.",
                        TransactionId = res.TransactionId,
                        OrderId = order.Id.ToString()
                    };
                }
            }

            return new PaymentResponseModel { Success = false, Message = "Thông tin giao dịch không hợp lệ." };
        }



    }
}
