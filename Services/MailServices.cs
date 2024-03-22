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
                           <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                    <div style='text-align: center; margin-bottom: 20px;'>
                        <h2 style='color: #0066cc; margin-bottom: 20px;'>MAIHOTEL</h2>
                    </div>
                    <div style='background-color: #f4f4f4; padding: 20px; border-radius: 10px;'>
                        <h2 style='color: #0066cc; margin-bottom: 20px; text-align: center;'>Đặt phòng thành công!</h2>
                        <div style='background-color: #ffffff; padding: 20px; border-radius: 10px;'>
                            <p><strong>Khách hàng:</strong> {exsitigUsers.FirstName} {exsitigUsers.LastName}</p>
                            <p><strong>Loại phòng:</strong> {room.Name}</p>
                            <p><strong>Tổng tiền:</strong> {booking.TotalAmount}</p>
                            <p><strong>Hình Thức Thanh Toán:</strong> VNPAY BANKING</p>
                            <p><strong>Ngày check-in:</strong> {booking.CheckInDateTime}</p>
                            <p><strong>Ngày check-out:</strong> {booking.CheckOutDateTime}</p>
                            <p><strong>Thời điểm đặt:</strong> {DateTime.Now}</p>
                        </div>
                        <p style='text-align: center; margin-top: 20px; color: #666;'>Nhân viên của chúng tôi sẽ xác nhận và phản hồi sớm nhất.</p>
                        <p style='text-align: center; color: #666;'>Email này được gửi từ một địa chỉ tự động và không thể nhận email phản hồi. Vui lòng không phản hồi lại email này.</p>
                    </div>
                    <div style='margin-top: 20px; background-color: #f4f4f4; padding: 20px; border-radius: 10px;'>
                        <p style='text-align: center; margin-bottom: 10px; font-weight: bold;'>Liên hệ:</p>
                        <p style='text-align: center; margin-bottom: 5px;'>No. 2 Pham Van Bach, Yen Hoa Ward,</p>
                        <p style='text-align: center; margin-bottom: 5px;'>Cau Giay District, Hanoi City</p>
                        <p style='text-align: center; margin-bottom: 5px;'>Điện thoại: <a href='tel:0965996180'>0965996180</a></p>
                        <p style='text-align: center;'>Email: <a href='mailto:thuanddgch200729@fpt.edu.vn'>thuanddgch200729@fpt.edu.vn</a></p>
                    </div>
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
