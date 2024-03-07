namespace DangDucThuanFinalYear.ApplicationHotel;

// Xử lí các trạng thái trả về chung của dự án
public readonly record struct HotelResult(bool IsSuccess, string? ErrorMessage)
{
    public static HotelResult Success() => new(true, null);
    public static HotelResult Errors(string errorMessage) => new(false, errorMessage);
    public static implicit operator HotelResult(bool isSuccess) => new(isSuccess, default);
    public static implicit operator HotelResult(string errorMessage) => Errors(errorMessage);

}

public record HotelResult<TData>(bool IsSuccess, string? ErrorMessage, TData Data)
    {
        public static HotelResult<TData> Success(TData data) => new ( true, null, data );
        public static HotelResult<TData> Errors(string errorMessage) => new(false, errorMessage, default);
        public static implicit operator HotelResult<TData>(TData data) => Success(data);
        public static implicit operator HotelResult<TData>(string errorMessage) => Errors(errorMessage);

    }

   


