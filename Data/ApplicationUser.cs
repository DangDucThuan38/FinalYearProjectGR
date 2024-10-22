using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DangDucThuanFinalYear.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [Required, MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required, MaxLength(10)]
        public string RoleName { get; set; }

        [Required, MaxLength(10), RegularExpression(@"[^0-9\+\(\)\s]")]
        public string ContactNumber { get; set; }
        [MaxLength(50),Unicode(false)]
        public string? Desgination { get; set; }
        [MaxLength(50), Unicode(false)]

        public string? Image { get; set; }
        [NotMapped]
        public string FullName=> $"{FirstName} {LastName}".Trim();
    }

}
