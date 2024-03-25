using DangDucThuanFinalYear.ApplicationHotel;
using DangDucThuanFinalYear.HotelDTO;

namespace DangDucThuanFinalYear.IServices
{
    public interface IBookingServices
    {
        Task<HotelResult<long>> MakeBookingAsync(BookingModel model, string userId);
    }
}
