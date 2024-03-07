using DangDucThuanFinalYear.ApplicationHotel;
using DangDucThuanFinalYear.Constants;
using DangDucThuanFinalYear.Data;
using DangDucThuanFinalYear.HotelDTO;

namespace DangDucThuanFinalYear.IServices
{
    public interface IUserService
    {
        Task<HotelResult<ApplicationUser>> CreateUserAsnyc(ApplicationUser user, string email, string passowrd);
        Task<UserInformation[]> GetUserInformationAsnyc(RoleType? roleType = null);
    }
}
