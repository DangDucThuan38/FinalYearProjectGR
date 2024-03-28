using DangDucThuanFinalYear.Constants;

namespace DangDucThuanFinalYear.HotelDTO.FinancesDTO
{
    public record SearchListFinaces(string Code, string NameFinance, string Reason, decimal Money, DateTime CreationTime);
}
