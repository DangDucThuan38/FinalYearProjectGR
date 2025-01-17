﻿using DangDucThuanFinalYear.ApplicationHotel;
using DangDucThuanFinalYear.Components.Account.Pages.Manage;
using DangDucThuanFinalYear.Data;
using DangDucThuanFinalYear.Data.Entities;
using DangDucThuanFinalYear.IServices;
using MailKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MailKit.Net.Smtp;
using DangDucThuanFinalYear.HotelDTO;
using DangDucThuanFinalYear.Constants;
using DocumentFormat.OpenXml.Spreadsheet;

namespace DangDucThuanFinalYear.Services
{
    public class MailServices : IMailServices
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly UserManager<ApplicationUser> _userManage;
        private readonly IUserStore<ApplicationUser> _userStore;

        public MailServices(IDbContextFactory<ApplicationDbContext> contextFactory,
                           UserManager<ApplicationUser> userManage,
                           IUserStore<ApplicationUser> userStore)
        {
            _contextFactory = contextFactory;
            _userManage = userManage;
            _userStore = userStore;
        }

        public async Task<bool> SendEmail(long? transactionRef, string userId)
        {
            using var context = _contextFactory.CreateDbContext();

            var exsitigUsers = await _userManage.FindByIdAsync(userId);
            var booking = await context.Boookings.FirstOrDefaultAsync(a => a.Id == transactionRef);
            var idroom = booking.RoomTypeId;
            var room = context.RoomTypes.FirstOrDefault(a => a.Id == idroom);
            if (exsitigUsers == null)
            {
                return false;
            }
            var mailUser = exsitigUsers.Email;
            string header = "[MAIHOTEL] XÁC NHẬN THANH TOÁN PHÒNG THÀNH CÔNG";
            string senderEmail = "dangducthuan999@gmail.com";
            string senderPassword = "cxsgusvpapbbnnpn";
            var total = booking.TotalAmount.ToString("#,##0");
            string recipientEmail = mailUser;
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(senderEmail, senderEmail));
            message.To.Add(new MailboxAddress("Recipient Name", recipientEmail));
            message.Subject = header;
            // Thêm nội dung email
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $@"
                   <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>

    <div style='background-color: #f4f4f4; padding: 20px; border-radius: 10px;'>
        <h2 style='color: green; margin-bottom: 20px; text-align: center;'>XÁC NHẬN THANH TOÁN ĐẶT PHÒNG</h2>
       <div style='background-color: #ffffff; padding: 20px; border-radius: 10px; position: relative;'>
        <p><strong>Khách Hàng:</strong> {exsitigUsers.FullName}</p>
        <p><strong>Loại phòng:</strong> {room.Name}</p>
        <P><strong>Mã Giao Dịch: MHTL{booking.Id}</strong></P>
        <p><strong>Tổng Tiền Thanh Toán:</strong> {total} VNĐ</p>
        <p><strong>Kênh Thanh Toán:</strong> VNPAY BANKING</p>
        <p><strong>Ngày Check-In:</strong> {booking.CheckInDateTime}</p>
        <p><strong>Ngày Check-Out:</strong> {booking.CheckOutDateTime}</p>
        <p><strong>Thời Điểm Đặt:</strong> {DateTime.Now}</p>
    <!-- Thay thế biểu tượng bằng chữ -->
    <span style=""color: red;  font-style: italic; position: absolute; top: 5px; right: 5px;"">MAIHOTEL Đã xác minh</span>

</div>

        <p style='text-align: center; margin-top: 20px; color: #666;'>Nhân viên của chúng tôi sẽ xác nhận và phản hồi sớm nhất.</p>
        <p style='text-align: center; color: #666;'>Email này được gửi từ một địa chỉ tự động và không thể nhận email phản hồi. Vui lòng không phản hồi lại email này.</p>
        <p style='text-align: center; margin-bottom: 10px; font-weight: bold;'>Liên hệ:</p>
        <p style='text-align: center; margin-bottom: 5px;'>Điện thoại: <a href='tel:0965996180'>0965996180</a></p>
        <p style='text-align: center;'>Email: <a href='mailto:thuanddgch200729@fpt.edu.vn'>thuanddgch200729@fpt.edu.vn</a></p>
        <p style='text-align: center; margin-bottom: 5px;'>Địa chỉ: No. 2 Pham Van Bach, Yen Hoa Ward, Cau Giay District, Hanoi City</p>
        
        <p style='text-align: center; margin-top: 20px; font-weight: bold;'>Trân trọng thông báo</p>
        <p style='text-align: center; margin-bottom: 5px; color: green;'>.MAI HOTEL.</p>
    </div>

</div>";

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate(senderEmail, senderPassword);
                client.Send(message);
                client.Disconnect(true);
            }
            Console.WriteLine("Email đã được gửi thành công!");
            return true;
        }

        public async Task<bool> SendEmailConfirmBooking(long bookingId)
        {
            using var context = _contextFactory.CreateDbContext();

            var booking = await context.Boookings.FirstOrDefaultAsync(a => a.Id == bookingId);
            var roomId = booking?.RoomId;
            var rooms = await context.Rooms.FirstOrDefaultAsync(a => a.Id == roomId);
            var guestId= booking?.GuestId;
            var exsitigUsers = await _userManage.FindByIdAsync(guestId);
            var idroom = booking.RoomTypeId;
            var roomtype = context.RoomTypes.FirstOrDefault(a => a.Id == idroom);
            if (exsitigUsers == null)
            {
                return false;
            }
            string? mailUser = exsitigUsers.Email;
            string header = "[MAIHOTEL] XÁC NHẬN PHÒNG ";
            string senderEmail = "dangducthuan999@gmail.com";
            string senderPassword = "cxsgusvpapbbnnpn";
            string recipientEmail = mailUser;
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(senderEmail, senderEmail));
            message.To.Add(new MailboxAddress("Recipient Name", recipientEmail));
            message.Subject = header;
            // Thêm nội dung email
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $@"
                   <body style='font-family: Arial, sans-serif;'>
    <div style='max-width: 600px; margin: 0 auto;'>
        <div style='background-color: #f4f4f4; padding: 20px; border-radius: 10px; text-align: center;'>
            <h2 style='color: green; margin-bottom: 20px;'>XÁC NHẬN PHÒNG</h2>
            <div style='background-color: #ffffff; padding: 20px; border-radius: 10px;'>
                <p><strong>Khách Hàng:</strong> {exsitigUsers.FullName}</p>
                <p><strong>Thời Điểm Đặt:</strong> {DateTime.Now}</p>
                <P><strong>Mã Giao Dịch: MHTL{booking.Id}</strong></P>
                <p><strong>Loại phòng:</strong> {roomtype.Name}</p>
                <p><strong>Số phòng:</strong> {rooms.RoomNumber}</p>
                <p><strong>Ngày Check-In:</strong> {booking.CheckInDateTime}</p>
                <p><strong>Ngày Check-Out:</strong> {booking.CheckOutDateTime}</p>
                <p style='color: red; font-style: italic;'>MAIHOTEL Đã xác minh</p>
            </div>
            <p style='color: #666;'>Email này được gửi từ một địa chỉ tự động và không thể nhận email phản hồi. Vui lòng không phản hồi lại email này.</p>
            <p style='margin-top: 20px; font-weight: bold;'>Liên hệ:</p>
            <p style='margin-bottom: 5px;'>Điện thoại: <a href='tel:0965996180'>0965996180</a></p>
            <p>Email: <a href='mailto:thuanddgch200729@fpt.edu.vn'>thuanddgch200729@fpt.edu.vn</a></p>
            <p style='margin-bottom: 5px;'>Địa chỉ: No. 2 Pham Van Bach, Yen Hoa Ward, Cau Giay District, Hanoi City</p>
            <p style='margin-top: 20px; font-weight: bold;'>Trân trọng thông báo</p>
            <p style='margin-bottom: 0; color: green;'>.MAI HOTEL.</p>
        </div>
    </div>
</body>";

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate(senderEmail, senderPassword);
                client.Send(message);
                client.Disconnect(true);
            }
            Console.WriteLine("Email đã được gửi thành công!");
            return true;
        }
    }
}