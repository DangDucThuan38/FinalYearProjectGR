using System.ComponentModel.DataAnnotations;

namespace DangDucThuanFinalYear.Data.Entities
{
    public class Boooking
    {
        [Key]
        public long Id { get; set; }
        public int RoomId { get; set; }
        [Required]
        public string GuestId { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
        public DateTime CheckInDateTime { get; set; }
        public DateTime CheckOutDateTime { get; set; }
        [Range(1, double.MaxValue)]
        public decimal TotalAmount { get; set; }
        public DateTime BookedOn { get; set; }
        public string? Remarks { get; set; }
        public virtual Room Room { get; set; }
        public virtual ApplicationUser Guest { get; set; }
    }

}
