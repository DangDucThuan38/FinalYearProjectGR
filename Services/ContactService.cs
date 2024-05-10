using DangDucThuanFinalYear.ApplicationHotel;
using DangDucThuanFinalYear.Data;
using DangDucThuanFinalYear.Data.Entities;
using DangDucThuanFinalYear.HotelDTO;
using DangDucThuanFinalYear.HotelDTO.ContactDTO;
using DangDucThuanFinalYear.IServices;
using Microsoft.EntityFrameworkCore;

namespace DangDucThuanFinalYear.Services
{
    public class ContactService : IContactService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public ContactService(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<HotelResult> AddContactAsync(ContactModelDTO input)
        {
            using var context = _contextFactory.CreateDbContext();
            var contact = new Contact
            {
                Name = input.Name,
                Message = input.Message,
                Email = input.Email,
                Subject = input.Subject,
                ContactOn = DateTime.Now,
            };
            await context.Contacts.AddAsync(contact);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<PageResult<Contact>> GetAllContactAsync(int startIndex,int pageSize)
        {
            using var context = _contextFactory.CreateDbContext();
            var totalCount = await context.Contacts.CountAsync();
            var result = await context.Contacts.OrderByDescending(x => x.ContactOn).Skip(startIndex).Take(pageSize).ToArrayAsync();

            return new PageResult<Contact>(totalCount, result);
        }    
    }

}
