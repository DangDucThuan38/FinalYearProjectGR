using System.ComponentModel.DataAnnotations;

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

        [Range(1, 100, ErrorMessage = "Please Select Room Type")]
        public short RoomTypeId { get; set; }
        [MaxLength(250)]
        public string? SpecialRequest { get; set; }


        public decimal Amount { get; set; }
        public void SetDummyValues()
        {
            Email = "dangducthuan999@gmail.com";
            ContactNumber = "0965996180";
            FirstName = "Dang";
            LastName = "DucThuan";
            Password = ConfirmPassword = "P@ss1234"; //
        }

        /*        public void SetValuesClaimsPrincipal(ClaimsPrincipal principal)
                {
                    if(principal?.Identity?.IsAuthenticated ==true )
                    {
                        var fullname = principal.FindFirstValue(AppConstants.CustomClaimTypes.FullName);
                        var roleName = principal.FindFirstValue(AppConstants.CustomClaimTypes.RoleName);
                         Email = principal.FindFirstValue(AppConstants.CustomClaimTypes.Email);
                        var userId = principal.GetUserId();
                         ContactNumber = principal.FindFirstValue(AppConstants.CustomClaimTypes.ContactNumber);

                        var parts = fullname.Split(' ');
                        FirstName = parts[0];
                        LastName = parts.Length > 1 ? parts[1] : null;

                        Password = ConfirmPassword = "P@ss1234"; //
                    }
         }*/
    }
}