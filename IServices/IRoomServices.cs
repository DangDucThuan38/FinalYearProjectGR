﻿using DangDucThuanFinalYear.HotelDTO;

namespace DangDucThuanFinalYear.IServices
{
    public interface IRoomServices
    {
        Task<RoomTyePublic[]> GetRoomTypeAsnyc(int count = 0);
    }
}