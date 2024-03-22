using DangDucThuanFinalYear.Components.Pages;

namespace DangDucThuanFinalYear.Data.Entities
{
    public class Payment
    {
        public Guid Id { get; set; }
        public long BookingId { get; set; }
        public string Name { get; set; }
        public string? AdditinoalInfo { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual Boooking Booking { get; set; }


    }
}
