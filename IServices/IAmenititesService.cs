using DangDucThuanFinalYear.ApplicationHotel;
using DangDucThuanFinalYear.Data.Entities;

namespace DangDucThuanFinalYear.IServices
{
    public interface IAmenititesService
    {
        // Lấy danh sách các Tiện ích của khách sạn
        Task<Amenity[]> GetAmenitiesAsync();
        // Thêm tiện ích của sách sạn
        Task<HotelResult<Amenity>> SaveAmenityAsync(Amenity amenity);
        //Xóa tiện ích của khách sạn
        Task<HotelResult<bool>> DeleteAmenitityAsyns(int amenityId);
    }
}
