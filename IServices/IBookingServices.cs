using DangDucThuanFinalYear.ApplicationHotel;
using DangDucThuanFinalYear.Constants;
using DangDucThuanFinalYear.Data.Entities;
using DangDucThuanFinalYear.HotelDTO;
using DangDucThuanFinalYear.HotelDTO.BookingDTO;

namespace DangDucThuanFinalYear.IServices
{
    public interface IBookingServices
    {
        Task<HotelResult<long>> MakeBookingAsync(BookingModel model, string userId);
    
        Task<PageResult<BookingDisplayModel>> GetBookingAsync(int pageIndex, int pageSize);
        Task<HotelResult> ApproveBookingAsync(long bookingId);
        Task<HotelResult> CancelBookingAsync(long bookingId,string cancelReason ,string? userId);
        Task<PageResult<BookingDisplayModel>> GetBookingByGuestAsync(string GuestId,BookingDisplayType type,int pageIndex, int pageSize);


    }
}
