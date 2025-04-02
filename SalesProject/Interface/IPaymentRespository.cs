using SalesProject.Models.Domain;

namespace SalesProject.Interface
{
    public interface IPaymentRespository
    {

        /// <summary>
        /// Tạo giao dịch VNPAY cho một đơn hàng.
        /// </summary>
        Task<VnPayCreatePaymentResult> CreateVnPayPaymentAsync(Guid orderId, string baseReturnUrl);

        /// <summary>
        /// Xử lý phản hồi từ VNPAY sau thanh toán.
        /// </summary>
        Task<bool> HandleVnPayReturnAsync(IQueryCollection queryParams);

        /// <summary>
        /// Tạo thanh toán nội bộ (ví dụ: tiền mặt).
        /// </summary>
        Task<Payment> CreateCashOnDeliveryPaymentAsync(Guid orderId);

    }
}
