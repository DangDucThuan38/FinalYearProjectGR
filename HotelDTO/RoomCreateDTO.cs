using DangDucThuanFinalYear.Data.Entities;
using DangDucThuanFinalYear.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace DangDucThuanFinalYear.HotelDTO
{
    public class RoomCreateDTO
    {
        public short Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; }

        [Required, MaxLength(200)]
        public string ImageUrl { get; set; }

        public IFormFile ImageFile { get; set; }


        [Required, Range(1, double.MaxValue)]
        public decimal Price { get; set; }

        [Required, MaxLength(200)]
        public string Descripcion { get; set; }
        [Range(1, 20)]
        public int MaxAults { get; set; }
        [Range(1, 10)]
        public int MaxChildren { get; set; }
        public bool IsActive { get; set; }
        public bool IsAvailable { get; set; }

        public RoomTypeAmenityCreateDTO[] Amenities { get; set; } = [];

        public class RoomTypeAmenityCreateDTO
        {
            public RoomTypeAmenityCreateDTO(int id, int? unit) => (Id, Unit) = (id, unit);
            public int Id { get; set; }
            public int? Unit { get; set; }
        }

    }
}
