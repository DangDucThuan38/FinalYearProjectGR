using DangDucThuanFinalYear.Data;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore;
using DangDucThuanFinalYear.ApplicationHotel;
using DangDucThuanFinalYear.HotelDTO;
using DangDucThuanFinalYear.IServices;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using DangDucThuanFinalYear.Data.Entities;

namespace DangDucThuanFinalYear.Services
{
    public class RoomService : IRoomService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public RoomService(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<HotelResult<short>> CreateRoomAsync(RoomCreateDTO input,string userId)
        {
            using var context = _contextFactory.CreateDbContext();
            if( await context.RoomTypes.AnyAsync(x=> x.Name == input.Name) ) 
            {
                return $" Room Type with the same {input.Name} already exists!";
            }
            var room = new RoomType
            {
                Name = input.Name,
                AddedBy = userId,
                CreationTime = DateTime.Now,
                Descripcion = input.Descripcion,
                ImageUrl = input.ImageUrl,
                IsActive = input.IsActive,
                MaxAults = input.MaxAults,
                MaxChildren = input.MaxChildren,
                Price = input.Price,
            };
            await context.RoomTypes.AddAsync(room);
            await context.SaveChangesAsync();
            if(input.Amenities.Length>0)
            {
                var roomTypeAmenities = input.Amenities.Select(x => new RoomTypeAmenity
                {
                    AmenityId = x.Id,
                    RoomTypeId = room.Id,
                    Unit = x.Id
                });
                await context.RoomTypeAmenitys.AddRangeAsync(roomTypeAmenities);
                await context.SaveChangesAsync();

            }
            return room.Id;


        }

        public async Task<SearchListRoomResults[]> GetRoomForManagePageResults()
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.RoomTypes
                .Select(x => new SearchListRoomResults(
                    x.Id,
                    x.Name,
                    x.ImageUrl,
                    x.Price
                    )).ToArrayAsync();

        }
    }
}
