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

        public async Task<RoomTyePublic[]> GetRoomTypeAsnyc(int count = 0, FilterModel? filterModel = null)
        {
            using var context = _contextFactory.CreateDbContext();
            var roomTypes = context.RoomTypes
                                .Where(x => x.IsActive);
                               
            if(filterModel is not null)
            {
                if(filterModel.Audlts>0)
                {
                    roomTypes = roomTypes.Where(x => x.MaxAults >= filterModel.Audlts);

                }
                if (filterModel.Children > 0)
                {
                    roomTypes = roomTypes.Where(x => x.MaxChildren >= filterModel.Children);

                }

            }    
                               
            if(count > 0 )
            {
                roomTypes = roomTypes.Take(count);
            }    
            return await roomTypes.Select(x =>
                                    new RoomTyePublic(
                                        x.Id,
                                        x.Name,
                                        x.ImageUrl,
                                        x.Descripcion,
                                        x.Price,
                                        x.RoomTypeAmenitys
                                            .Select(a => new RoomTypeAmenityModel(a.Amenity.Name, a.Amenity.Icon, a.Unit))
                                            .ToArray()
                                    )).ToArrayAsync();
        }

        public async Task<LookupModel<short>[]> GetRoomTypeLookupAsnyc()
        {
            using var context = _contextFactory.CreateDbContext();
        
             return await context.RoomTypes
                .Where(x=>x.IsActive)
                .Select(a=> new LookupModel<short> (a.Id,a.Name))
                .ToArrayAsync();
        }


    }
}