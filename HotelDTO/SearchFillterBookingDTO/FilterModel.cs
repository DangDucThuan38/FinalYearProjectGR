using Microsoft.AspNetCore.Components;

namespace DangDucThuanFinalYear.HotelDTO
{
    public class FilterModel
    {
        public FilterModel()
        {
            
        }
        public static FilterModel GetFilterModel(DateOnly? checkInDate, DateOnly? checkOutDate, int? audlts, int? children)
        {
            return new()
            {
                CheckInDate = checkInDate,
                CheckOutDate = checkOutDate,
                Audlts = audlts,
                Children = children,
            };
           
        }

        public DateOnly? CheckInDate { get; set; } 
        public DateOnly? CheckOutDate { get; set; } 
        public int? Audlts { get; set; } = 0;
        public int? Children { get; set; } = 0;
        public IReadOnlyDictionary<string, object?> ToDictionary() =>
            new Dictionary<string, object?>
            {
                [nameof(CheckInDate)] = CheckInDate,
                [nameof(CheckOutDate)] = CheckOutDate,
                [nameof(Audlts)] = Audlts,
                [nameof(Children)] = Children,

            };



    }
}
