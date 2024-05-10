using DangDucThuanFinalYear.Constants;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DangDucThuanFinalYear.HotelDTO.BookingDTO
{
    public class BookingDisplayModel
    {
        public long Id { get; set; }

        public short RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }
        public int? RoomId { get; set; }
        public string? RoomNumber { get; set; }
        public string GuestId { get; set; }
        public string GuestName { get; set; }
        public int Adults { get; set; }
        public int? Children { get; set; }
        public DateOnly CheckInDateTime { get; set; }
        public DateOnly CheckOutDateTime { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime BookedOn { get; set; }
        public string? Remarks { get; set; }
        public string? SpecialRequest { get; set; }
        public BookingStatus? BookingStatus { get; set; }
        public string? FullName { get; set; }
        public bool IsRoomNumberAssignable => BookingStatus == Constants.BookingStatus.Paid || BookingStatus == Constants.BookingStatus.Booked;

        public bool CanBeApproved => BookingStatus == Constants.BookingStatus.Paid;
        public bool CanBeCancelled => BookingStatus == Constants.BookingStatus.Unpaid && BookingStatus != Constants.BookingStatus.Cancelled;

        public bool CanMakePayment => (BookingStatus == Constants.BookingStatus.Pendding || BookingStatus == Constants.BookingStatus.Unpaid)
                                        && CheckInDateTime >= DateOnly.FromDateTime(DateTime.Today);



    }
}
