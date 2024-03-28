using DangDucThuanFinalYear.Constants;
using DangDucThuanFinalYear.Data;
using DangDucThuanFinalYear.Data.Entities;
using DangDucThuanFinalYear.HotelDTO.DashboardDTO;
using DangDucThuanFinalYear.HotelDTO.UserProFileDTO;
using DangDucThuanFinalYear.IServices;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DangDucThuanFinalYear.Services
{
    public class RevenueDashboardService :IRevenueDashboardService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly UserManager<ApplicationUser> _userManage;


        public RevenueDashboardService(IDbContextFactory<ApplicationDbContext> contextFactory, UserManager<ApplicationUser> userManage)
        {
            _contextFactory = contextFactory;
            _userManage = userManage;
        }

        public async Task<DashboardRespon> GetRevenueDashboardAsync()
        {
            using var context = _contextFactory.CreateDbContext();
            var booking = await context.Boookings.Where(a => a.BookingStatus == Constants.BookingStatus.PaymentDone ).ToListAsync();
            var totalRevenue = booking.Sum(a => a.TotalAmount);
            var totalBooking = booking.Count;
            var query = await _userManage.Users.Where(x=>x.RoleName == RoleType.Guest.ToString()).ToListAsync();
            var totalGuest = query.Count;
            var staff = await _userManage.Users.Where(x => x.RoleName == RoleType.Staff.ToString()).ToListAsync();
            var totalStaff = staff.Count;
            var Totalroom = await context.Rooms.Where(a => a.IsDeleted == false).ToListAsync();
            var TotalRoom = Totalroom.Count;
          

            var result = new DashboardRespon
            {
                TotalRevenue = totalRevenue,
                TotalBooking = totalBooking,
                TotalCustomer = totalGuest,
                TotalStaff = totalStaff,
                TotalRoom = TotalRoom
                
            };
            return result;
        }
    }
}
