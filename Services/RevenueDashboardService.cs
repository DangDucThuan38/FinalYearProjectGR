﻿using DangDucThuanFinalYear.Constants;
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

            // Truy vấn booking và tính tổng doanh thu
            var bookingQuery = context.Boookings.Where(a => a.BookingStatus == Constants.BookingStatus.PaymentDone);
            var totalRevenue = await bookingQuery.SumAsync(a => a.TotalAmount);

            // Truy vấn và tính tổng các phiếu chi
            var totalChi = await context.Finances
                                         .Where(a => a.NameFinance == "Phiếu Chi" && !a.IsDeleted)
                                         .SumAsync(a => a.Money);

            // Truy vấn và tính tổng các phiếu thu
            var totalThu = await context.Finances
                                         .Where(a => a.NameFinance == "Phiếu Thu" && !a.IsDeleted)
                                         .SumAsync(a => a.Money);

            // Tính tổng thu
            var thu = totalThu - totalChi;

            // Truy vấn và tính số lượng booking, khách hàng, nhân viên, phòng
            var totalBooking = await bookingQuery.CountAsync();
            var totalGuest = await _userManage.Users.Where(x => x.RoleName == RoleType.Guest.ToString()).CountAsync();
            var totalStaff = await _userManage.Users.Where(x => x.RoleName == RoleType.Staff.ToString()).CountAsync();
            var totalRoom = await context.Rooms.Where(a => !a.IsDeleted).CountAsync();

            // Tạo đối tượng DashboardRespon và trả về
            var result = new DashboardRespon
            {
                TotalRevenue = totalRevenue + thu,
                TotalBooking = totalBooking,
                TotalCustomer = totalGuest,
                TotalStaff = totalStaff,
                TotalRoom = totalRoom,
                TotalChi = totalChi,
                TotalThu = totalThu
            };
            return result;
        }

    }
}
