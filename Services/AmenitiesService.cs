using DangDucThuanFinalYear.ApplicationHotel;
using DangDucThuanFinalYear.Data;
using DangDucThuanFinalYear.Data.Entities;
using DangDucThuanFinalYear.IServices;
using Microsoft.EntityFrameworkCore;

namespace DangDucThuanFinalYear.Services
{
  
    public class AmenitiesService : IAmenititesService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public AmenitiesService(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<HotelResult<bool>> DeleteAmenitityAsyns(int amenityId)
        {
            using var context =_contextFactory.CreateDbContext();
            var amenity = context.Amenitys.FirstOrDefault(a => a.Id == amenityId);
            if(amenity is not null)
            {
                context.Entry(amenity).State = EntityState.Modified;
                amenity.IsDeleted = true;
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Amenity[]> GetAmenitiesAsync()
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Amenitys.Where(a=>a.IsDeleted == false).ToArrayAsync();


        }

        public async Task<HotelResult<Amenity>> SaveAmenityAsync(Amenity amenity)
        {
            using var context = _contextFactory.CreateDbContext();
            if (amenity.Id == 0)
            {
                //Create new
                if(await context.Amenitys.AnyAsync(a=>a.Name == amenity.Name && a.IsDeleted == false))
                {
                    //return HotelResult<Amenity>.Errors("Amenity exists already");
                    return "Amenity exists already";
                }

                await context.Amenitys.AddAsync(amenity);
            }
            else
            {
                //Update
                if (await context.Amenitys.AnyAsync(a => a.Name == amenity.Name && a.Id != amenity.Id))
                {
                    return "Amenity not Found";
                }
                var dbAmenity = await context.Amenitys
                    .AsTracking()
                    .FirstOrDefaultAsync(a => a.Id == amenity.Id)
                ?? throw new InvalidOperationException("Not Found Amenity");
                dbAmenity.Name = amenity.Name;
                dbAmenity.Icon = amenity.Icon;


            }
            await context.SaveChangesAsync();
            return amenity;
        }
    }
}
