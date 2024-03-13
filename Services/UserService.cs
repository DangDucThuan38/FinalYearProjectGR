using DangDucThuanFinalYear.ApplicationHotel;
using DangDucThuanFinalYear.Constants;
using DangDucThuanFinalYear.Data;
using DangDucThuanFinalYear.HotelDTO;
using DangDucThuanFinalYear.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DangDucThuanFinalYear.Services
{
    public class UserService : IUserService
    { 

        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly UserManager<ApplicationUser> _userManage;
        private readonly IUserStore<ApplicationUser> _userStore;

        public UserService(IDbContextFactory<ApplicationDbContext> contextFactory,
                           UserManager<ApplicationUser> userManage,
                           IUserStore<ApplicationUser> userStore)
        {
            _contextFactory = contextFactory;
            _userManage = userManage;
            _userStore = userStore;
        }

        public async Task<HotelResult<ApplicationUser>> CreateUserAsnyc(ApplicationUser user,string email,string passowrd)
        {
            await _userStore.SetUserNameAsync(user, email, CancellationToken.None);
            var emailStore = GetEmailStore();
            await emailStore.SetEmailAsync(user, email, CancellationToken.None);
            var result = await _userManage.CreateAsync(user, passowrd);

            if (!result.Succeeded)
            {
                return  $"Error: {string.Join(", ", result.Errors.Select(error => error.Description))}";
                
            }

            result = await _userManage.AddToRoleAsync(user,user.RoleName ?? RoleType.Guest.ToString());
            if (!result.Succeeded)
            {
                return $"Error: {string.Join(", ", result.Errors.Select(error => error.Description))}";
            }
            return user;
        }

        public async Task<PageResult<UserInformation>> GetUserInformationAsnyc(int startIndex, int PageSize, RoleType? roleType = null)
        {
            var query = _userManage.Users;
            if(roleType is not null)
            {
                query = query.Where(x => x.RoleName == roleType.ToString());
            }
            var total = await query.CountAsync();
            var recors = await query.Select(x => new UserInformation(x.Id, x.FullName, x.Email, x.RoleName,x.ContactNumber,x.Desgination))
                .Skip(startIndex)
                .Take(PageSize)
                .ToArrayAsync();

            return new PageResult<UserInformation>(total, recors);
            
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManage.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }

        public async Task<UdateUserModel?> GetStaffMemberAsync(string staffId) =>
            await GetStaff(staffId)
                            .Select(x => new UdateUserModel
                            {
                                Id = x.Id,
                                ContactNumber = x.ContactNumber,
                                Desgination = x.Desgination,
                                Email = x.Email,
                                FirstName = x.FirstName,
                                LastName = x.LastName,

                            }).FirstOrDefaultAsync();


        private IQueryable<ApplicationUser> GetStaff(string staffId) =>
            _userManage.Users.Where(x => x.Id == staffId && x.RoleName == RoleType.Staff.ToString());


        public async Task<HotelResult> UpdateStaffAsnyc(UdateUserModel input)
        {
            var staffmodel = await GetStaff(input.Id).FirstOrDefaultAsync();
            if(staffmodel is null)
            {
                return "Invalid Request.Please Try Agian!!!";
            }

            staffmodel.FirstName = input.FirstName;
            staffmodel.LastName = input.LastName;
            staffmodel.Desgination = input.Desgination;
            staffmodel.ContactNumber = input.ContactNumber;

            var result = await _userManage.UpdateAsync(staffmodel);
            if (!result.Succeeded)
            {
                return $"Error: {string.Join(", ", result.Errors.Select(error => error.Description))}";
            }
            if(staffmodel.Email.Equals(input.Email, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

           var token = await _userManage.GenerateChangeEmailTokenAsync(staffmodel, input.Email);

           result = await _userManage.ChangeEmailAsync(staffmodel, input.Email, token);
            if (!result.Succeeded)
            {
                return $"Error: {string.Join(", ", result.Errors.Select(error => error.Description))}";
            }
            return true;

        }
    }
}
