using DangDucThuanFinalYear.Constants;
using DangDucThuanFinalYear.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Server.Kestrel.Transport.NamedPipes;

namespace DangDucThuanFinalYear.Services
{
public class SeedServices
{
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public SeedServices(UserManager<ApplicationUser> userManager,
                              IUserStore<ApplicationUser> userStore,
                              RoleManager<IdentityRole> roleManager,
                              IConfiguration configuration)
        {
            _userManager = userManager;
            _userStore = userStore;
            _roleManager = roleManager;
            _configuration = configuration;
        }
        public async Task SeedDatabaseAsync()
        {
            var adminUserEmail = _configuration.GetValue<string>("AdminHotel:Email");
            var dbAdminUser = await _userManager.FindByEmailAsync(adminUserEmail);
            if (dbAdminUser is not null)
                return;
            var applicationUser = new ApplicationUser()
            {

               FirstName = _configuration.GetValue<string>("AdminHotel:FirstName")!,
               LastName = _configuration.GetValue<string>("AdminHotel:LastName")!,
               RoleName = RoleType.Admin.ToString(),
               ContactNumber = _configuration.GetValue<string>("AdminHotel:Contact")!,
               Desgination = "Adminstrator",
            };
            await _userStore.SetUserNameAsync(applicationUser, adminUserEmail, default);
            var emailStore = (IUserEmailStore<ApplicationUser>)_userStore;
            await emailStore.SetEmailAsync(applicationUser, adminUserEmail, default);

            var result = await _userManager.CreateAsync(applicationUser , _configuration.GetValue<string>("AdminHotel:Password")!);
            if(!result.Succeeded)
            {
                var errors = string.Join(Environment.NewLine, result.Errors.Select(e=>e.Description));
                throw new Exception($"Error in creating user.{errors}");
            }    
            if(await _roleManager.FindByNameAsync(RoleType.Admin.ToString()) is null)
            {
                foreach (var item in Enum.GetNames<RoleType>())
                {
                    var role = new IdentityRole
                    {
                        Name = item,
                    };
                    await _roleManager.CreateAsync(role);
                }
            }
             result = await _userManager.AddToRoleAsync(applicationUser, RoleType.Admin.ToString());
            if (!result.Succeeded)
            {
                var errors = string.Join(Environment.NewLine, result.Errors.Select(e => e.Description));
                throw new Exception($"Error in adding user to Admin role.{errors}");
            }

        }
    }
}
           
      

