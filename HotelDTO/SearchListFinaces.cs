using DangDucThuanFinalYear.Constants;

namespace DangDucThuanFinalYear.HotelDTO
{
    public record SearchListFinaces(string Code, TypeFinance TypeFinance, string Reason, decimal Money, DateTime CreationTime);
}
