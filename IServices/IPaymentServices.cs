using DangDucThuanFinalYear.Constants;
using DangDucThuanFinalYear.HotelDTO;

namespace DangDucThuanFinalYear.IServices
{
    public interface IPaymentServices
    {
        public Task<string> VNPayPayment(long bookingId, BookingModel model);
        public Task MakPayment(BookingModel model, string userId);
        public Task<string> PaymentWithVNPay(long? transactionRef, string userId,BookingStatus bookingStatus,string? statusPayment);

    }
}
