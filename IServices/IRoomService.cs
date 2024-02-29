using DangDucThuanFinalYear.ApplicationHotel;
using DangDucThuanFinalYear.HotelDTO;

namespace DangDucThuanFinalYear.IServices
{
    public interface IRoomService
    {
        Task<HotelResult<short>> CreateRoomAsync(RoomTypeCreateDTO input, string userId);
        Task<SearchListRoomTypeResults[]> GetRoomForManagePageResults();
    }
}
