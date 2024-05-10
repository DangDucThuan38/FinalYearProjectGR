using DangDucThuanFinalYear.HotelDTO;
using DangDucThuanFinalYear.Data.Entities;
using DangDucThuanFinalYear.IServices;
using Microsoft.EntityFrameworkCore;
using DangDucThuanFinalYear.Data;
using DangDucThuanFinalYear.ApplicationHotel;
using DangDucThuanFinalYear.HotelDTO.BookingDTO;
using DangDucThuanFinalYear.Constants;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;

namespace DangDucThuanFinalYear.Services
{
    public class BookingServices : IBookingServices
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly UserManager<ApplicationUser> _userManage;

        public BookingServices(IDbContextFactory<ApplicationDbContext> contextFactory,
                                       UserManager<ApplicationUser> userManage
)
        {
            _contextFactory = contextFactory;
            _userManage = userManage;

        }

        public async Task<HotelResult<long>> MakeBookingAsync(BookingModel model,string userId)
        {
            try
            {
                var exsitigUsers = await _userManage.FindByIdAsync(userId);
                var fullName = exsitigUsers.FullName;
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
                    TotalAmount = model.Amount,
                    NameCustomer = fullName,
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


        public async Task<PageResult<BookingDisplayModel>> SearchBookingAsync(int pageIndex, int pageSize, string name)
        {
           
            using var context = _contextFactory.CreateDbContext();
            var query = context.Boookings;
            var totalCount = await query.CountAsync();

            var bookingsQuery = query.OrderByDescending(x => x.CheckInDateTime)
                          .Select(x => new BookingDisplayModel
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
                              RoomNumber = x.RoomId == null ? "" : x.Room.RoomNumber,
                              RoomTypeName = x.RoomType.Name,
                              GuestName = x.Guest.FullName,
                              Remarks = x.Remarks,
                              FullName = x.NameCustomer,
                          })
                          .Skip((pageIndex))
                          .Take(pageSize);

            if (!string.IsNullOrEmpty(name))
            {
                bookingsQuery = bookingsQuery.Where(x => x.FullName == name);
                 totalCount = await bookingsQuery.CountAsync();

            }

            var bookings = await bookingsQuery.ToArrayAsync();

            return new PageResult<BookingDisplayModel>(totalCount, bookings);
        }




        public async Task<HotelResult> ApproveBookingAsync(long bookingId)
        {
            using var context = _contextFactory.CreateDbContext();
            var bookingApprove = await context.Boookings
                .AsTracking()
                .FirstOrDefaultAsync(x => x.Id == bookingId);
            if (bookingApprove is null)
            {
                return "Invalid Request";
            }
            switch (bookingApprove.BookingStatus)
            {
                case Constants.BookingStatus.Booked:
                    return "Booking already approved";
                case Constants.BookingStatus.Cancelled:
                    return "Booking already cancelled";
                case Constants.BookingStatus.Paid:
                    bookingApprove.BookingStatus = Constants.BookingStatus.Booked;
                    break;
                default:
                    return "Invalid Request";
            }
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<HotelResult> CancelBookingAsync(long bookingId, string cancelReason, string? userId)
        {
            using var context = _contextFactory.CreateDbContext();
            var bookingCanncel= await context.Boookings
                .AsTracking()
                .FirstOrDefaultAsync(x => x.Id == bookingId);
            if (bookingCanncel is null)
            {
                return "Invalid Request";
            }
            if(bookingCanncel.BookingStatus == Constants.BookingStatus.Cancelled)
                    return "Already Cancelled";

            bookingCanncel.BookingStatus = Constants.BookingStatus.Cancelled;
            bookingCanncel.Remarks += Environment.NewLine + $"Cancelled by{(userId == bookingCanncel.GuestId ?"Guest":"Staff?Admin")} Staff/Admin. Resaon:{cancelReason}";
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<PageResult<BookingDisplayModel>> GetBookingByGuestAsync(string GuestId, BookingDisplayType type, int pageIndex, int pageSize)
        {
            using var context = _contextFactory.CreateDbContext();
            var query = context.Boookings.Where(x=>x.GuestId == GuestId);
            var now = DateOnly.FromDateTime(DateTime.Now);
            query = type switch
            {
                BookingDisplayType.Upcoming => query.Where(x => x.CheckInDateTime > now),
                BookingDisplayType.Ongoing => query.Where(x => x.CheckInDateTime == now || x.CheckOutDateTime == now),
                BookingDisplayType.Past => query.Where(x => x.CheckInDateTime < now),

            };

            var totalCount = await query.CountAsync();
            var bookings = await query.OrderByDescending(x => x.CheckInDateTime)
                .Select(BookingDisplayModelSelector)
                .Skip(pageIndex)
                .Take(pageSize)
                .ToArrayAsync();
            return new PageResult<BookingDisplayModel>(totalCount, bookings);
        }

        public async Task<int> CheckAvailableRoomBooking(BookingModel model)
        {
            var checkIn = (model.CheckInDate);
            var checkOut = (model.CheckOutDate);
            using var context = _contextFactory.CreateDbContext();
            var room = await context.Rooms.Where(x => x.RoomTypeId == model.RoomTypeId).CountAsync();
            var booking = await context.Boookings
                .Where(x => x.RoomTypeId == model.RoomTypeId && x.CheckInDateTime <= checkOut && x.CheckOutDateTime >= checkIn)
                .CountAsync();
            var roomAvaliable = room - booking;
            return roomAvaliable;
        }

        private static Expression<Func<Boooking, BookingDisplayModel>> BookingDisplayModelSelector = 
            x => new BookingDisplayModel
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
            RoomNumber = x.RoomId == null ? "" : x.Room.RoomNumber,
            RoomTypeName = x.RoomType.Name,
            GuestName = x.Guest.FullName,
            Remarks = x.Remarks
        };

    }
}
