using DangDucThuanFinalYear.HotelDTO;

namespace DangDucThuanFinalYear.IServices
{
    public interface IRoomServices
    {
        Task<RoomTyePublic[]> GetRoomTypeAsnyc(int count = 0, FilterModel? filterModel= null);
        Task<LookupModel<short>[]> GetRoomTypeLookupAsnyc();
    }
}
