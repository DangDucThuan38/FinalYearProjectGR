using DangDucThuanFinalYear.ApplicationHotel;
using DangDucThuanFinalYear.Data.Entities;
using DangDucThuanFinalYear.HotelDTO.RoomTypeDTO;

namespace DangDucThuanFinalYear.IServices
{
    public interface IRoomTypeService
    {
        // Tạo Loại phòng mới của khách sạn
        Task<HotelResult<short>> CreateRoomAsync(RoomTypeCreateUpDateDTO input, string userId);

        // Get all loại phòng của khách sạn
        Task<SearchListRoomTypeResults[]> GetRoomForManagePageResults();

        //Edit loại phòng của khách sạn
        Task<RoomTypeCreateUpDateDTO> GetRoomTypeEditAsync(short id);

        //Get all danh sác phòng theo loại phòng của khách sạn
        Task<Room[]> GetRoomsAllAsync(short roomTypeId);
        // Tạo phòng mới theo loại phòng
        Task<HotelResult<Room>> SaveRoomAsync(Room room);
        // Xóa phòng 
        Task<HotelResult> DeleteRoomAsync(int roomId);

        // Assgin Room to User
        Task<HotelResult> AssignRoomToUserAsync(long bookingId, int roomId);

    }
}
