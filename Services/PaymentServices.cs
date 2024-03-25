using ASPNetCoreAPIDemo;
using Azure;
using DangDucThuanFinalYear.Components.Pages;
using DangDucThuanFinalYear.Constants;
using DangDucThuanFinalYear.Data;
using DangDucThuanFinalYear.Data.Entities;
using DangDucThuanFinalYear.HotelDTO;
using DangDucThuanFinalYear.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Asn1.X9;
using System.Globalization;

namespace DangDucThuanFinalYear.Services
{
    public class PaymentServices : IPaymentServices
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManage;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PaymentServices(IDbContextFactory<ApplicationDbContext> contextFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManage)
        {
            _contextFactory = contextFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _userManage = userManage;
        }

        public Task MakPayment(BookingModel model, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<string> VNPayPayment(long bookingId, BookingModel model)
        {
            //Get Config Info
            string vnp_Returnurl = _configuration.GetSection("AppSettings:ReturnVnPayUrl").Value!; //URL nhan ket qua tra ve 
            string vnp_Url = _configuration.GetSection("AppSettings:TransacVnPayUrl").Value!; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = _configuration.GetSection("AppSettings:TmnCode").Value!; //Ma định danh merchant kết nối (Terminal Id)
            string vnp_HashSecret = _configuration.GetSection("AppSettings:HashSecret").Value!; //Secret Key
            
            ////Get payment input
            //OrderInfo order = new OrderInfo();
            ////Save order to db
            //order.OrderId = DateTime.Now.Ticks; // Giả lập mã giao dịch hệ thống merchant gửi sang VNPAY
            //order.Amount = 100000; // Giả lập số tiền thanh toán hệ thống merchant gửi sang VNPAY 100,000 VND
            //order.Status = "0"; //0: Trạng thái thanh toán "chờ thanh toán" hoặc "Pending"
            //order.OrderDesc = txtOrderDesc.Text;
            //order.CreatedDate = DateTime.Now;
            //string locale = cboLanguage.SelectedItem.Value;
            



            //Build URL for VNPAY
            VNpayLibrary vnpay = new VNpayLibrary();
            vnpay.AddRequestData("vnp_Version", "2.1.0");
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            decimal exchangeRateUSDToVND = 23000;
            decimal amountInVND = model.Amount * exchangeRateUSDToVND * 100;
            vnpay.AddRequestData("vnp_Amount", amountInVND.ToString("F0", CultureInfo.InvariantCulture));
            //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000

            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", "127.0.0.1");
            var locale = string.Empty;

            vnpay.AddRequestData("vnp_Locale", !string.IsNullOrEmpty(locale) ? locale : "vn");
            vnpay.AddRequestData("vnp_OrderInfo", $"Thanh Toán Booking Hotel Code {bookingId}");
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", bookingId.ToString()); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày
            //Add Params of 2.1.0 Version
            vnpay.AddRequestData("vnp_ExpireDate", string.Empty);
            //Billing
            vnpay.AddRequestData("vnp_Bill_Mobile", string.Empty);
            vnpay.AddRequestData("vnp_Bill_Email", string.Empty);
            var fullName = string.Empty;
            if (!String.IsNullOrEmpty(fullName))
            {
                var indexof = fullName.IndexOf(' ');
                vnpay.AddRequestData("vnp_Bill_FirstName", fullName.Substring(0, indexof));
                vnpay.AddRequestData("vnp_Bill_LastName", fullName.Substring(indexof + 1, fullName.Length - indexof - 1));
            }
            vnpay.AddRequestData("vnp_Bill_Address", string.Empty);
            vnpay.AddRequestData("vnp_Bill_City", string.Empty);
            vnpay.AddRequestData("vnp_Bill_Country", string.Empty);
            vnpay.AddRequestData("vnp_Bill_State", "");
            // Invoice
            vnpay.AddRequestData("vnp_Inv_Phone", string.Empty);
            vnpay.AddRequestData("vnp_Inv_Email", string.Empty);
            vnpay.AddRequestData("vnp_Inv_Customer", string.Empty);
            vnpay.AddRequestData("vnp_Inv_Address", string.Empty);
            vnpay.AddRequestData("vnp_Inv_Company", string.Empty);
            vnpay.AddRequestData("vnp_Inv_Taxcode", string.Empty);
            vnpay.AddRequestData("vnp_Inv_Type", string.Empty);

            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            return Task.FromResult(paymentUrl);
        }

        public VNPay VnPayReturn()
        {
            string vnp_HashSecret = _configuration.GetSection("AppSettings:HashSecret").Value!;
            var vnpayData = _httpContextAccessor.HttpContext.Request.Query;
            VNpayLibrary vnpay = new VNpayLibrary();

            foreach (var key in vnpayData.Keys)
            {
                string value = vnpayData[key];
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value);
                }
            }

            int OrderId = int.Parse(vnpay.GetResponseData("vnp_TxnRef"));
            long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
            string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
            string vnp_SecureHash = _httpContextAccessor.HttpContext.Request.Query["vnp_SecureHash"];
            String TerminalID = _httpContextAccessor.HttpContext.Request.Query["vnp_TmnCode"];
            float vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
            String bankCode = _httpContextAccessor.HttpContext.Request.Query["vnp_BankCode"];

            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
            string displayMsg, displayTmnCode, displayTxnRef, displayVnpayTranNo, displayAmount, displayBankCode;


            try
            {
                using var context = _contextFactory.CreateDbContext();
                var findOrder = context.Boookings.FirstOrDefault(a => a.Id == OrderId);
                if (findOrder == null)
                {
                    return new VNPay
                    {
                        OrderId = (int)OrderId,
                        vnpayTranId = (int)vnpayTranId,
                        vnp_ResponseCode = vnp_ResponseCode,
                        vnp_TransactionStatus = vnp_TransactionStatus,
                        vnp_Amount = vnp_Amount,
                        displayMsg = "Not Found"
                    };


                }
                if (findOrder != null)
                {
                    Console.WriteLine("Ok");
                }


                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                    {
                        displayMsg = "Giao dịch được thực hiện thành công. Cảm ơn quý khách đã sử dụng dịch vụ";
                        findOrder.BookingStatus = Constants.BookingStatus.Booked;
                    }
                    else
                    {
                        //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                        displayMsg = "Có lỗi xảy ra trong quá trình xử lý.Mã lỗi: " + vnp_ResponseCode;
                        findOrder.BookingStatus = Constants.BookingStatus.Cancelled;
                        return new VNPay
                        {
                            OrderId = (int)OrderId,
                            vnpayTranId = (int)vnpayTranId,
                            vnp_ResponseCode = vnp_ResponseCode,
                            vnp_TransactionStatus = vnp_TransactionStatus,
                            vnp_Amount = vnp_Amount,
                            displayMsg = displayMsg

                        };
                    }
                    displayTmnCode = "Mã Website (Terminal ID):" + TerminalID;
                    displayTxnRef = "Mã giao dịch thanh toán:" + OrderId.ToString();
                    displayVnpayTranNo = "Mã giao dịch tại VNPAY:" + vnpayTranId.ToString();
                    displayAmount = "Số tiền thanh toán (VND):" + vnp_Amount.ToString();
                    displayBankCode = "Ngân hàng thanh toán:" + bankCode;
                    context.Update(findOrder);
                    context.SaveChanges();

                    return new VNPay
                    {
                        OrderId = (int)OrderId,
                        vnpayTranId = (int)vnpayTranId,
                        vnp_ResponseCode = vnp_ResponseCode,
                        vnp_TransactionStatus = vnp_TransactionStatus,
                        vnp_Amount = vnp_Amount,
                        displayMsg = displayMsg,
                        displayTmnCode = displayTmnCode,
                        displayBankCode = displayBankCode,
                        displayTxnRef = displayTxnRef,
                        displayVnpayTranNo = displayVnpayTranNo,
                        displayAmount = displayAmount,
                        Status = findOrder.BookingStatus.ToString()
                    };
                }
                else
                {
                    displayMsg = "Có lỗi xảy ra trong quá trình xử lý";

                    return new VNPay
                    {
                        OrderId = (int)OrderId,
                        vnpayTranId = (int)vnpayTranId,
                        vnp_ResponseCode = vnp_ResponseCode,
                        vnp_TransactionStatus = vnp_TransactionStatus,
                        vnp_Amount = vnp_Amount,
                        displayMsg = displayMsg
                    };
                }
            }
            catch (Exception ex)
            {
                return new VNPay
                {
                    Status = ex.Message
                };

            }




        }

        public async Task<string> PaymentWithVNPay(long? transactionRef, string userId,BookingStatus bookingStatus, string? statusPayment)
        {
            using var context = _contextFactory.CreateDbContext();

            var exsitigUsers = await _userManage.FindByIdAsync(userId);
            var booking = await context.Boookings.FirstOrDefaultAsync(a => a.Id == transactionRef);
            var idroom = booking.RoomTypeId;
            var room = context.RoomTypes.FirstOrDefault(a => a.Id == idroom);
            //Update Status Booking
            booking.BookingStatus = bookingStatus;
            context.UpdateRange(booking);
            await context.SaveChangesAsync();
            try
            {
                var payment = new Payment
                {
                    BookingId = booking.Id,
                    Name = exsitigUsers.FirstName + " " + exsitigUsers.LastName,
                    AdditinoalInfo = "VNPAY QR BANKING",
                    Status = statusPayment,
                    CreatedAt = DateTime.Now,
                };
                await context.Payments.AddAsync(payment);
                await context.SaveChangesAsync();
                return "Payment";
            }
            catch (Exception ex)
            {
                throw;
            }

            throw new NotImplementedException();
        }



    }
}
