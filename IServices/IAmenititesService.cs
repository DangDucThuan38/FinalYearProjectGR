using DangDucThuanFinalYear.ApplicationHotel;
using DangDucThuanFinalYear.Data.Entities;

namespace DangDucThuanFinalYear.IServices
{
    public interface IAmenititesService
    {
        Task<Amenity[]> GetAmenitiesAsync();
        Task<HotelResult<Amenity>> SaveAmenityAsync(Amenity amenity);
        Task<HotelResult<bool>> DeleteAmenitityAsyns(int id);
    }
}
