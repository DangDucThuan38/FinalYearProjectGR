using DangDucThuanFinalYear.HotelDTO;
using DangDucThuanFinalYear.Data.Entities;
using DangDucThuanFinalYear.IServices;
using Microsoft.EntityFrameworkCore;
using DangDucThuanFinalYear.Data;
using DangDucThuanFinalYear.ApplicationHotel;
using DangDucThuanFinalYear.HotelDTO.BookingDTO;

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



        public async Task<PageResult<BookingDisplayModel>> GetBookingAsync(int pageIndex, int pageSize)
        {
            using var context = _contextFactory.CreateDbContext();
            var query = context.Boookings;

            var totalCount = await query.CountAsync();
            var bookings = await query.OrderByDescending(x=>x.CheckInDateTime)
                .Select( x=> new BookingDisplayModel
                {
                    Id = x.Id,
                    Adults = x.Adults,
                    BookedOn = x.BookedOn,
                    CheckInDateTime = x.CheckInDateTime,
                    CheckOutDateTime = x.CheckOutDateTime,
                    Children = x.Children,
                    GuestId = x.GuestId,
                    RoomId = x.RoomId,
                    RoomTypeId = x.RoomTypeId,
                    SpecialRequest = x.SpecialRequest,
                    TotalAmount = x.TotalAmount,
                    BookingStatus = x.BookingStatus,
                    RoomNumber = x.RoomId== null?"":x.Room.RoomNumber,
                    RoomTypeName = x.RoomType.Name,
                    GuestName = x.Guest.FullName,
                    Remarks = x.Remarks
                })
                .Skip(pageIndex)
                .Take(pageSize)
                .ToArrayAsync();
            return new PageResult<BookingDisplayModel>(totalCount, bookings);
        }

    }
}
