namespace DangDucThuanFinalYear.HotelDTO
{
    public record PageResult<TData>(int TotalCount, TData[] Records);
    
}
