using DangDucThuanFinalYear.HotelDTO;
using DangDucThuanFinalYear.Data.Entities;
using DangDucThuanFinalYear.IServices;
using Microsoft.EntityFrameworkCore;
using DangDucThuanFinalYear.Data;
using DangDucThuanFinalYear.ApplicationHotel;

namespace DangDucThuanFinalYear.Services
{
    public class BookingServices : IBookingServices
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public BookingServices(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<HotelResult<long>> MakeBookingAsync(BookingModel model,string userId)
        {
            try
            {
                var booking = new Boooking
                {
                    Adults = model.Audlts ?? 0,
                    BookedOn = DateTime.Now,
                    CheckInDateTime = model.CheckInDate,
                    CheckOutDateTime = model.CheckOutDate,
                    Children = model.Children,
                    GuestId = userId,
                    RoomTypeId = model.RoomTypeId,
                    SpecialRequest = model.SpecialRequest,
                    BookingStatus = Constants.BookingStatus.Pendding,
                    TotalAmount = model.Amount
                };
                using var context = _contextFactory.CreateDbContext();
                await context.Boookings.AddAsync(booking);
                await context.SaveChangesAsync();
                return booking.Id;
                

            }
            catch(Exception ex)
            {
                throw;
            }
           

        }


    }
}
