using DangDucThuanFinalYear.Constants;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DangDucThuanFinalYear.HotelDTO
{
    public class BookingDisplayModel
    {
        public long Id { get; set; }

        public short RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }
        public int? RoomId { get; set; }
        public string? RoomNumber { get; set;} 
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
        public bool IsRoomNumberAssignable => BookingStatus == Constants.BookingStatus.PaymentDone || BookingStatus == Constants.BookingStatus.Booked;

        public bool CanBeApproved => BookingStatus == Constants.BookingStatus.PaymentDone;
        public bool CanBeCancelled => BookingStatus == Constants.BookingStatus.PaymentCanneled && BookingStatus !=Constants.BookingStatus.Cancelled;
    }
}
