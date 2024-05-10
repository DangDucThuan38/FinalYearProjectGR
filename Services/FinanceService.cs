using DangDucThuanFinalYear.ApplicationHotel;
using DangDucThuanFinalYear.Constants;
using DangDucThuanFinalYear.Data;
using DangDucThuanFinalYear.Data.Entities;
using DangDucThuanFinalYear.HotelDTO.FinancesDTO;
using DangDucThuanFinalYear.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace DangDucThuanFinalYear.Services
{
    public class FinanceService : IFinanceService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly UserManager<ApplicationUser> _userManage;
        private readonly IUserStore<ApplicationUser> _userStore;
        public FinanceService(IDbContextFactory<ApplicationDbContext> contextFactory,
                           UserManager<ApplicationUser> userManage,
                           IUserStore<ApplicationUser> userStore)
        {
            _contextFactory = contextFactory;
            _userManage = userManage;
            _userStore = userStore;
        }
        public async Task<HotelResult<bool>> DeleteAmenitityAsyns(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            var result = context.Finances.FirstOrDefault(a => a.Id == id);
            if (result is not null)
            {
                context.Entry(result).State = EntityState.Modified;
                result.IsDeleted = true;
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Finances[]> GetFinancesAllAsync()
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Finances.Where(a => a.IsDeleted == false).ToArrayAsync();
        }
     


        public async Task<SearchListFinaces[]> GetFinancesAsync()
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Finances
                .Select(x => new SearchListFinaces(
                    x.Code,
                    x.NameFinance,
                    x.Reason,
                    x.Money,
                    x.CreationTime
                    )).ToArrayAsync();
        }

        public Task<Finances[]> GetFinancesSearchAsync(string? name)
        {
            using var context = _contextFactory.CreateDbContext();
            var finances = context.Finances.Where(a => a.IsDeleted == false).ToArrayAsync();
            if (name != null)
            {
                finances = context.Finances.Where(a => a.Code == name || a.NameFinance == name && a.IsDeleted == false).ToArrayAsync();
            }
            
                return finances;
        }

        public async Task<HotelResult<Finances>> SaveFinanceAsync(Finances input, string userId)
        {
            using var context = _contextFactory.CreateDbContext();
             var exsitigUsers = await _userManage.FindByIdAsync(userId);
            var addBY = exsitigUsers.FullName;
            Finances? finances;
            if (input.Id == 0)
            {
                int existingCount = await context.Finances.CountAsync();
                string uniqueCode = $"PTC{existingCount + 1}";

                //Create new
                if (await context.Finances.AnyAsync(a => a.Code == input.Code && a.IsDeleted == false))
                {
                    return "Finances exists already";
                }
                finances = new Finances
                {
                    Id = input.Id,
                    Code = uniqueCode,
                    NameFinance = input.NameFinance,
                    Reason = input.Reason,
                    Money = input.Money,
                    Descripcion = input.Descripcion,
                    CreationTime = DateTime.Now,
                    AddedBy = addBY
                };
                await context.Finances.AddAsync(finances);
            }
            else
            {
                //if (await context.Finances.AnyAsync(a => a.Id != input.Id))
                //{
                //    return "Finances not Found";
                //}
                var dbFinances = await context.Finances
                    .AsTracking()
                    .FirstOrDefaultAsync(a => a.Id == input.Id)
                ?? throw new InvalidOperationException("Not Found Amenity");
                dbFinances.NameFinance = input.NameFinance;
                dbFinances.Reason = input.Reason;
                dbFinances.Money = input.Money;
                dbFinances.Descripcion = input.Descripcion;
                dbFinances.LastUpdatedBy = addBY;
                dbFinances.LastUpdated = DateTime.Now;
            }
            await context.SaveChangesAsync();
            return input;
        }


    }
}
