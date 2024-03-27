using DangDucThuanFinalYear.Migrations;

namespace DangDucThuanFinalYear.HotelDTO.RoomTypeDTO
{

    public readonly record struct RoomTypeAmenityModel(string name, string? Icon = null, int? Unit = null);

    public record RoomTyePublic(short id, string Name, string Image, string Description, decimal Price, RoomTypeAmenityModel[] Amenities);


}
