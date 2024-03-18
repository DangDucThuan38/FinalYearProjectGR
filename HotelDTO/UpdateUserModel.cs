using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DangDucThuanFinalYear.HotelDTO
{
    public class UdateUserModel
    {
        public string Id {  get; set; }
        [Required, MaxLength(50)]
        public string FirstName { get; set; } = "";

        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = "";

        [Required, MaxLength(10)]
        public string ContactNumber { get; set; }

        [MaxLength(50), Unicode(false)]
        public string? Desgination { get; set; }

    }
}
