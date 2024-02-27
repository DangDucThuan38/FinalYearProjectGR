using DangDucThuanFinalYear.Components.Pages;
using DangDucThuanFinalYear.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DangDucThuanFinalYear.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Entities.Room> Rooms { get; set; }
        public DbSet<Entities.RoomType> RoomTypes { get; set; }
        public DbSet<Entities.Amenity> Amenitys { get; set; }
        public DbSet<Entities.RoomTypeAmenity> RoomTypeAmenitys { get; set; }
        public DbSet<Entities.Boooking> Boookings { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<RoomTypeAmenity>()
                .HasKey(x => new { x.RoomTypeId, x.AmenityId });
            builder.Entity<RoomType>()
                .HasMany(rt => rt.Rooms)
                .WithOne(r=> r.RoomType)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
