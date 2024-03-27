using DangDucThuanFinalYear.Constants;

namespace DangDucThuanFinalYear.HotelDTO.FinancesDTO
{
    public record SearchListFinaces(string Code, TypeFinance TypeFinance, string Reason, decimal Money, DateTime CreationTime);
}
