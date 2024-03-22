using DangDucThuanFinalYear.ApplicationHotel;
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
        public async Task<bool> SendEmail(BookingModel model, string userId)
        {
            using var context = _contextFactory.CreateDbContext();

            var exsitigUsers = await _userManage.FindByIdAsync(userId);
            var room = context.RoomTypes.FirstOrDefault(a => a.Id == model.RoomTypeId);
            if (exsitigUsers == null)
            {
                return false;
            }
            var mailUser = exsitigUsers.Email;
            string header = "[MAIHOTEL] XÁC NHẬN THANH TOÁN PHÒNG THÀNH CÔNG";
            string senderEmail = "dangquocphongbni98@gmail.com";
            string senderPassword = "wrmbdzmlnqhxjdzb";
            string recipientEmail = mailUser;
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(senderEmail, senderEmail));
            message.To.Add(new MailboxAddress("Recipient Name", recipientEmail));
            message.Subject = header;
            // Thêm nội dung email
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $@"
                <div style='font-family: Arial, sans-serif;'>
                    <h2 style='color: #0066cc;'>Bạn đã đặt phòng thành công!</h2>
                    <p><strong>Name Booking:</strong> {model.LastName}</p>
                    <p><strong>Loại phòng:</strong> {room.Name}</p>
                    <p><strong>Tổng tiền:</strong> {model.Amount}</p>
                    <p><strong>Ngày check-in:</strong> {model.CheckInDate}</p>
                    <p><strong>Ngày check-out:</strong> {model.CheckOutDate}</p>
                    <p><strong>Giờ đặt:</strong> {DateTime.Now}</p>
                    <p>Nhân viên của chúng tôi sẽ xác nhận và phản hồi sớm nhất.</p>
                    <br/>
                    <p style='color: #666;'>Email này được gửi từ một địa chỉ tự động và không thể nhận email phản hồi. Vui lòng không phản hồi lại email này.</p>
                </div>
            ";
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
