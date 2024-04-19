using DangDucThuanFinalYear.HotelDTO;

namespace DangDucThuanFinalYear.IServices
{
    public interface IMailServices
    {
        Task<bool> SendEmail(long? transactionRef, string userId);
        Task<bool> SendEmailConfirmBooking(long bookingId);


    }
}
