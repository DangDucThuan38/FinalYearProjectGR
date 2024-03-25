using DangDucThuanFinalYear.Data;
using DangDucThuanFinalYear.Data.Entities;
using DangDucThuanFinalYear.HotelDTO;
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

        public async Task<RoomTyePublic[]> GetRoomTypeAsnyc(int count = 0, FilterModel? filterModel = null)
        {
            using var context = _contextFactory.CreateDbContext();
            var query = ApplyFilters(context.RoomTypes, filterModel);
                               
            if(count > 0 )
            {
                query = query.Take(count);
            }    
            return await query.Select(x =>
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

            }
            return query;
        }


    }
}