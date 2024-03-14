using DangDucThuanFinalYear.Data;
using DangDucThuanFinalYear.HotelDTO;
using DangDucThuanFinalYear.IServices;
using Microsoft.EntityFrameworkCore;

namespace DangDucThuanFinalYear.Services
{
    public class RoomServices : IRoomServices
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public RoomServices(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<RoomTyePublic[]> GetRoomTypeAsnyc(int count = 0)
        {
            using var context = _contextFactory.CreateDbContext();
            var roomTypes =  context.RoomTypes
                                .Where(x => x.IsActive)
                                .Select(x =>
                                    new RoomTyePublic(
                                        x.Id,
                                        x.Name,
                                        x.ImageUrl,
                                        x.Descripcion,
                                        x.Price,
                                        x.RoomTypeAmenitys
                                            .Select(a => new RoomTypeAmenityModel(a.Amenity.Name, a.Amenity.Icon, a.Unit))
                                            .ToArray()
                                    ));
                               
            if(count > 0 )
            {
                roomTypes = roomTypes.Take(count);
            }    
            return await roomTypes.ToArrayAsync();
        }
    }
}