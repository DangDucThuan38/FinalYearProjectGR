using DangDucThuanFinalYear.ApplicationHotel;
using DangDucThuanFinalYear.Data.Entities;
using DangDucThuanFinalYear.HotelDTO;

namespace DangDucThuanFinalYear.IServices
{
    public interface IBookingServices
    {
        Task<HotelResult<long>> MakeBookingAsync(BookingModel model, string userId);
    
        Task<PageResult<BookingDisplayModel>> GetBookingAsync(int pageIndex, int pageSize);
    }
}
