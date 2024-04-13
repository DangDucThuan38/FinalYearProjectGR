using DangDucThuanFinalYear.HotelDTO.DashboardDTO;

namespace DangDucThuanFinalYear.IServices
{
    public interface IRevenueDashboardService
    {
        Task<DashboardRespon> GetRevenueDashboardAsync();
        Task<List<DashboardResponByRoomType>> GetRevenueDashboardRoomTypeAsync();

    }
}
