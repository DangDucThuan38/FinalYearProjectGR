using System.ComponentModel.DataAnnotations;

namespace DangDucThuanFinalYear.HotelDTO.ContactDTO
{
    public class ContactModelDTO
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }
        [Required, MaxLength(100), EmailAddress]
        public string Email { get; set; }
        [Required, MaxLength(250)]
        public string Subject { get; set; }

        [Required, MaxLength(250)]
        public string Message { get; set; }
    }
}
