using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DangDucThuanFinalYear.Data.Entities
{
    public class RoomType
    {
        public short Id { get; set; }
        [Required, MaxLength(200)]
        public string Name { get; set; }

        [Required, MaxLength(200)]
        public string ImageUrl { get; set; }

        [Required, Range(1, double.MaxValue)]
        public decimal Price { get; set; }

        [Required, MaxLength(900000)]
        public string Descripcion { get; set; }
        public int MaxAults { get; set; }
        public int MaxChildren { get; set; }

        public bool IsActive { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime CreationTime { get; set; }
        public string AddedBy { get; set; }
        public DateTime LastUpdated { get; set; }
        public string? LastUpdatedBy { get; set; }
        public int View { get; set; }

        [ForeignKey(nameof(AddedBy))]
        public virtual ApplicationUser AddByUser { get; set; }
        public virtual ICollection<RoomTypeAmenity> RoomTypeAmenitys { get; set; }= new List<RoomTypeAmenity>();
        public virtual ICollection<Room> Rooms { get; set; }


    }


}
