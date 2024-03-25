using DangDucThuanFinalYear.Constants;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DangDucThuanFinalYear.Data.Entities
{
    public class Boooking
    {
        [Key]
        public long Id { get; set; }

        public short RoomTypeId { get; set; }
        public int? RoomId { get; set; }
        [Required]
        public string GuestId { get; set; }
        public int Adults { get; set; }
        public int? Children { get; set; }
        public DateOnly CheckInDateTime { get; set; }
        public DateOnly CheckOutDateTime { get; set; }
        [Range(1, double.MaxValue)]
        public decimal TotalAmount { get; set; }
        public DateTime BookedOn { get; set; }
        public string? Remarks { get; set; }

        [MaxLength(250),Unicode(false)]
        public string? SpecialRequest { get; set; }


        public BookingStatus? BookingStatus { get; set; } = Constants.BookingStatus.Pendding;
        public virtual Room Room { get; set; }
        public virtual RoomType RoomType { get; set; }

        public virtual ApplicationUser Guest { get; set; }
    }

}
