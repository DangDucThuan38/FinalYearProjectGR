using DangDucThuanFinalYear.ApplicationHotel;
using DangDucThuanFinalYear.Data.Entities;
using DangDucThuanFinalYear.HotelDTO;
using DangDucThuanFinalYear.HotelDTO.ContactDTO;

namespace DangDucThuanFinalYear.IServices
{
    public interface IContactService
    {
        public Task<HotelResult> AddContactAsync(ContactModelDTO input);
        public Task<PageResult<Contact>> GetAllContactAsync(int startIndex, int pageSize);

    }
}
