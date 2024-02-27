using DangDucThuanFinalYear.ApplicationHotel;
using DangDucThuanFinalYear.HotelDTO;

namespace DangDucThuanFinalYear.IServices
{
    public interface IRoomService
    {
        Task<HotelResult<Guid>> CreateRoomAsync(RoomCreateDTO input, string userId);
    }
}
