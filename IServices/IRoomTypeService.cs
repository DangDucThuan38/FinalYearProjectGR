using DangDucThuanFinalYear.ApplicationHotel;
using DangDucThuanFinalYear.Data.Entities;
using DangDucThuanFinalYear.HotelDTO;

namespace DangDucThuanFinalYear.IServices
{
    public interface IRoomTypeService
    {
        Task<HotelResult<short>> CreateRoomAsync(RoomTypeCreateUpDateDTO input, string userId);
        Task<SearchListRoomTypeResults[]> GetRoomForManagePageResults();
        Task<RoomTypeCreateUpDateDTO> GetRoomTypeEditAsync(short id);
        Task<Room[]> GetRoomsAllAsync(short roomTypeId);
        Task<HotelResult<Room>> SaveRoomAsync(Room room);
        Task<HotelResult> DeleteRoomAsync(int roomId);

    }
}
