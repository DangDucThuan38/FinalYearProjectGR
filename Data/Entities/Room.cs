using System.ComponentModel.DataAnnotations;

namespace DangDucThuanFinalYear.Data.Entities
{
    public class Room
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(25)]
        public string RoomNumber { get; set; }
        public short RoomTypeId { get; set; }
        public bool IsAvaiable { get; set; }
        public bool IsDeleted { get; set; }

        public virtual RoomType RoomType { get; set; }

    }


}
