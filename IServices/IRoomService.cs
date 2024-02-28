using DangDucThuanFinalYear.ApplicationHotel;
using DangDucThuanFinalYear.HotelDTO;

namespace DangDucThuanFinalYear.IServices
{
    public interface IRoomService
    {
        Task<HotelResult<short>> CreateRoomAsync(RoomCreateDTO input, string userId);
        Task<SearchListRoomResults[]> GetRoomForManagePageResults();
    }
}
