using System.ComponentModel.DataAnnotations;

namespace DangDucThuanFinalYear.Data.Entities
{
    public class Amenity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, MaxLength(100)]
        public string Icon { get; set; }
    }


}
