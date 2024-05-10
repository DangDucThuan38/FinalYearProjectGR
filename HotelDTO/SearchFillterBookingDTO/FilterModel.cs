using Microsoft.AspNetCore.Components;

namespace DangDucThuanFinalYear.HotelDTO
{
    public class FilterModel
    {
        public FilterModel()
        {
            
        }
        public static FilterModel GetFilterModel(string? name, int? audlts, int? children)
        {
            return new()
            {
                Name = name,
                Audlts = audlts,
                Children = children,
            };
           
        }

        public string? Name { get; set; } 
        public int? Audlts { get; set; } = 0;
        public int? Children { get; set; } = 0;
        public IReadOnlyDictionary<string, object?> ToDictionary() =>
            new Dictionary<string, object?>
            {
                [nameof(Name)] = Name,
                [nameof(Audlts)] = Audlts,
                [nameof(Children)] = Children,

            };



    }
    public class Search {
        public Search()
        {

        }
        public static Search GetSearch(string? name)
        {
            return new()
            {
                Name = name,
            };

        }

        public string? Name { get; set; }
        public IReadOnlyDictionary<string, object?> ToDictionary() =>
            new Dictionary<string, object?>
            {
                [nameof(Name)] = Name,
            };
    }



}
