﻿using DangDucThuanFinalYear.ApplicationHotel;
using DangDucThuanFinalYear.Data.Entities;
using DangDucThuanFinalYear.HotelDTO.FinancesDTO;

namespace DangDucThuanFinalYear.IServices
{
    public interface IFinanceService
    {
        Task<HotelResult<Finances>> SaveFinanceAsync(Finances input, string userId);
        Task<HotelResult<bool>> DeleteAmenitityAsyns(int id);
        Task<SearchListFinaces[]> GetFinancesAsync();
        Task<Finances[]> GetFinancesAllAsync();
        Task<Finances[]> GetFinancesSearchAsync(string? name);


    }
}
