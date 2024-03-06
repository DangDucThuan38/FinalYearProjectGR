using DangDucThuanFinalYear.ApplicationHotel;
using DangDucThuanFinalYear.HotelDTO;

namespace DangDucThuanFinalYear.IServices
{
    public interface IRoomTypeService
    {
        Task<HotelResult<short>> CreateRoomAsync(RoomTypeCreateUpDateDTO input, string userId);
        Task<SearchListRoomTypeResults[]> GetRoomForManagePageResults();
        Task<RoomTypeCreateUpDateDTO> GetRoomTypeEditAsync(short id);

    }
}
