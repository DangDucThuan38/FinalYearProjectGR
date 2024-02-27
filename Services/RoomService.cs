using DangDucThuanFinalYear.Data;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore;
using DangDucThuanFinalYear.ApplicationHotel;
using DangDucThuanFinalYear.HotelDTO;
using DangDucThuanFinalYear.IServices;

namespace DangDucThuanFinalYear.Services
{
    public class RoomService : IRoomService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public RoomService(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<HotelResult<Guid>> CreateRoomAsync(RoomCreateDTO input,string userId)
        {
            using var context = _contextFactory.CreateDbContext();
            return "âsasasa";
        }    
    }
}
