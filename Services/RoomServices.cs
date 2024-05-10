using DangDucThuanFinalYear.Data;
using DangDucThuanFinalYear.Data.Entities;
using DangDucThuanFinalYear.HotelDTO;
using DangDucThuanFinalYear.HotelDTO.RoomTypeDTO;
using DangDucThuanFinalYear.HotelDTO.ShowRoomPublicDTO;
using DangDucThuanFinalYear.IServices;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
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

        //public async Task<RoomTyePublic[]> GetRoomTypeAsnyc(int count = 0, FilterModel? filterModel = null)
        //{
        //    using var context = _contextFactory.CreateDbContext();
        //    var query = ApplyFilters(context.RoomTypes, filterModel);
                               
        //    if(count > 0 )
        //    {
        //        query = query.Take(count);
        //    }    
        //    return await query.Select(x =>
        //                            new RoomTyePublic(
        //                                x.Id,
        //                                x.Name,
        //                                x.ImageUrl,
        //                                x.Descripcion,
        //                                x.Price,
        //                                x.View,
        //                                x.RoomTypeAmenitys
        //                                    .Select(a => new RoomTypeAmenityModel(a.Amenity.Name, a.Amenity.Icon, a.Unit))
        //                                    .ToArray()
        //                            )).ToArrayAsync();
        //}
        public async Task<RoomTyePublic[]> GetRoomTypeAsnyc(int count = 0, FilterModel? filterModel = null)
        {
            using var context = _contextFactory.CreateDbContext();
            var query = ApplyFilters(context.RoomTypes, filterModel);

            if (count > 0)
            {
                query = query.Take(count);
            }

            // Include the count of bookings for each room type
            var roomTypeBookingsCounts = await context.Boookings
                .GroupBy(b => b.RoomTypeId)
                .Select(g => new { RoomTypeId = g.Key, BookingCount = g.Count() })
                .ToDictionaryAsync(x => x.RoomTypeId, x => x.BookingCount);

            return await query.Select(x =>
                new RoomTyePublic(
                    x.Id,
                    x.Name,
                    x.ImageUrl,
                    x.Descripcion,
                    x.Price,
                    x.View,
                    x.RoomTypeAmenitys
                        .Select(a => new RoomTypeAmenityModel(a.Amenity.Name, a.Amenity.Icon, a.Unit))
                        .ToArray(),
                    roomTypeBookingsCounts.ContainsKey(x.Id) ? roomTypeBookingsCounts[x.Id] : 0 
                )).ToArrayAsync();
        }



        public async Task<RoomTyePublicDetalis> GetRoomTypeDetailsAsnyc(short id)
        {
            using var context = _contextFactory.CreateDbContext();

            var roomType = await context.RoomTypes
                                        .Include(rt => rt.RoomTypeAmenitys)
                                            .ThenInclude(rta => rta.Amenity)
                                        .FirstOrDefaultAsync(rt => rt.Id == id);

            if (roomType != null)
            {
                return new RoomTyePublicDetalis(
                    roomType.Id,
                    roomType.Name,
                    roomType.ImageUrl,
                    roomType.Descripcion,
                    roomType.Price,
                    roomType.MaxAults,
                    roomType.View,
                    roomType.MaxChildren,
                    roomType.RoomTypeAmenitys
                        .Select(a => new RoomTypeAmenityModel(a.Amenity.Name, a.Amenity.Icon, a.Unit))
                        .ToArray()
                );
            }
            else
            {
                return null;
            }
        }


        public async Task<LookupModel<short,decimal>[]> GetRoomTypeLookupAsnyc(FilterModel? filter = null)
        {
            using var context = _contextFactory.CreateDbContext();
            var query = ApplyFilters(context.RoomTypes, filter);
            return await query.Select(a => new LookupModel<short,decimal>(a.Id, a.Name, a.Price))
                           .ToArrayAsync();
        }


        private static IQueryable<RoomType> ApplyFilters(IQueryable<RoomType> q, FilterModel? filter = null)
        {
            var query = q.Where(x => x.IsActive);


            if (filter is not null)
            {
                if (filter.Audlts > 0)
                {
                    query = query.Where(x => x.MaxAults >= filter.Audlts);

                }
                if (filter.Children > 0)
                {
                    query = query.Where(x => x.MaxChildren >= filter.Children);

                }
                if(filter.Name !=null)
                {
                    query = query.Where(x => x.Name == filter.Name);

                }

            }
            return query;
        }

        public async Task<int> GetRoomCount(short id)
        {
            using var context = _contextFactory.CreateDbContext();
            var dbroomType = await context.RoomTypes
                  .AsTracking()
                  .FirstOrDefaultAsync(a => a.Id == id)
              ?? throw new InvalidOperationException("Not Found Amenity");
            dbroomType.View += 1;
            await context.SaveChangesAsync();

            return dbroomType.View;
        }

    }
}