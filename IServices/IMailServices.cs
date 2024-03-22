using DangDucThuanFinalYear.HotelDTO;

namespace DangDucThuanFinalYear.IServices
{
    public interface IMailServices
    {
        Task<bool> SendEmail(BookingModel model,string userId);

    }
}
