using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DangDucThuanFinalYear.Data.Entities
{
    public class Contact
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required,MaxLength(100),Unicode(false)]
        public string Name { get; set; }
        [Required, MaxLength(100),EmailAddress, Unicode(false)]
        public string Email { get; set; }
        [Required, MaxLength(250), Unicode(false)]
        public string Subject { get; set; }
        
        [Required, MaxLength(250)]
        public string Message { get; set; }
        public DateTime ContactOn { get; set; }
    }
}
