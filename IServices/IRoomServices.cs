using DangDucThuanFinalYear.HotelDTO;
using DangDucThuanFinalYear.HotelDTO.RoomTypeDTO;
using DangDucThuanFinalYear.HotelDTO.ShowRoomPublicDTO;

namespace DangDucThuanFinalYear.IServices
{
    public interface IRoomServices
    {
        Task<RoomTyePublic[]> GetRoomTypeAsnyc(int count = 0, FilterModel? filterModel= null);
        Task<LookupModel<short, decimal>[]> GetRoomTypeLookupAsnyc(FilterModel? filter = null);
        Task<RoomTyePublicDetalis> GetRoomTypeDetailsAsnyc(short id);
        Task<int> GetRoomCount(short id);

    }
}
