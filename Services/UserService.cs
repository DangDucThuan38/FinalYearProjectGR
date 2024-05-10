using DangDucThuanFinalYear.ApplicationHotel;
using DangDucThuanFinalYear.Constants;
using DangDucThuanFinalYear.Data;
using DangDucThuanFinalYear.HotelDTO;
using DangDucThuanFinalYear.HotelDTO.UserProFileDTO;
using DangDucThuanFinalYear.IServices;
using DangDucThuanFinalYear.Migrations;
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
            var exsitigUsers =  await _userManage.FindByEmailAsync(email);
            if(exsitigUsers is not null)
            {
                return new HotelResult<ApplicationUser>(false,"Email already exists", exsitigUsers);
            }

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

        public async Task<MyProfileModel?> GetProfileDetailsAsync(string userId) =>
            await GetUser(userId)
                            .Select(x => new MyProfileModel
                            {
                                Id = x.Id,
                                ContactNumber = x.ContactNumber,
                                Desgination = x.Desgination,
                                Email = x.Email,
                                FirstName = x.FirstName,
                                LastName = x.LastName,

                            }).FirstOrDefaultAsync();


        private IQueryable<ApplicationUser> GetUser(string userId, RoleType? roleType=null)
        {
            var query = _userManage.Users.Where(x => x.Id == userId);

            if (roleType is not null)
            {
                query = query.Where(x=> x.RoleName == RoleType.Staff.ToString());
            }
            return query;
            

        }


        public async Task<HotelResult> UpdateProfileAsnyc(MyProfileModel input, RoleType? roleType = null)
        {
            var user = await GetUser(input.Id,roleType)
                    .FirstOrDefaultAsync();
            if(user is null)
            {
                return "Invalid Request.Please Try Agian!!!";
            }

            user.FirstName = input.FirstName;
            user.LastName = input.LastName;
            if (input.Desgination != null)
            {
                user.Desgination = input.Desgination;
            }
            user.ContactNumber = input.ContactNumber;

            var result = await _userManage.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return $"Error: {string.Join(", ", result.Errors.Select(error => error.Description))}";
            }
            if(user.Email.Equals(input.Email, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

           var token = await _userManage.GenerateChangeEmailTokenAsync(user, input.Email);

           result = await _userManage.ChangeEmailAsync(user, input.Email, token);
            if (!result.Succeeded)
            {
                return $"Error: {string.Join(", ", result.Errors.Select(error => error.Description))}";
            }
            return true;

        }

        public async Task<HotelResult> ChangePasswordAsnyc(ChangePasswordDTO input, string userId)
        {
            var user = await _userManage.FindByIdAsync(userId);
            if (user is null)
                return "Invalid request";
            var result = await _userManage.ChangePasswordAsync(user, input.CurrentPasswords, input.NewPassword);
            if(!result.Succeeded)
            {
                return $"Error: {string.Join(", ", result.Errors.Select(error => error.Description))}";
            }
            return true;
        }
    }
}
