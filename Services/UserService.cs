using DangDucThuanFinalYear.ApplicationHotel;
using DangDucThuanFinalYear.Constants;
using DangDucThuanFinalYear.Data;
using DangDucThuanFinalYear.HotelDTO;
using DangDucThuanFinalYear.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DangDucThuanFinalYear.Services
{
    public class UserService: IUserService 
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



        public async Task<UserInformation[]> GetUserInformationAsnyc(RoleType? roleType = null)
        {
            var query = _userManage.Users;
            if(roleType is not null)
            {
                query = query.Where(x => x.RoleName == roleType.ToString());
            }
            return await query.Select(x => new UserInformation(x.Id, x.FullName, x.Email, x.RoleName,x.ContactNumber,x.Desgination)).ToArrayAsync();
            
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManage.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
