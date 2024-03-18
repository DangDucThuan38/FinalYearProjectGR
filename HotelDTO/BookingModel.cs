using DangDucThuanFinalYear.Constants;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace DangDucThuanFinalYear.HotelDTO
{
    public class BookingModel
    {
        [Required, MaxLength(15)]
        public string FirstName { get; set; } = "";

        [MaxLength(15)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = "";

        [Required, MaxLength(10)]
        public string ContactNumber { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = "";

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = "";

        public DateOnly CheckInDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);
        public DateOnly CheckOutDate { get; set; } = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
        public int? Audlts { get; set; } = 0;
        public int? Children { get; set; } = 0;

        [Range(1,100, ErrorMessage ="Please Select Room Type")]
        public short RoomTypeId { get; set; }



        //public void SetValuesClaimsPrincipal(ClaimsPr principal)
        //{
        //    var fullname = principal.FindFirstValue(AppConstants.CustomClaimTypes.FullName);
        //    var roleName = principal.FindFirstValue(AppConstants.CustomClaimTypes.RoleName);
        //    var email = principal.FindFirstValue(AppConstants.CustomClaimTypes.Email);
        //    var userId = principal.
        //}

    }
}
