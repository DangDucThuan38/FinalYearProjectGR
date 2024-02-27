namespace DangDucThuanFinalYear.ApplicationHotel
{
    public record HotelResult<TData>(bool IsSuccess, string? ErrorMessage, TData Data)
    {
        public static HotelResult<TData> Success(TData data) => new ( true, null, data );
        public static HotelResult<TData> Errors(string errorMessage) => new(false, errorMessage, default);
        public static implicit operator HotelResult<TData>(TData data) => Success(data);
        public static implicit operator HotelResult<TData>(string errorMessage) => Errors(errorMessage);

    }

}
