﻿using DangDucThuanFinalYear.Data;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore;
using DangDucThuanFinalYear.ApplicationHotel;
using DangDucThuanFinalYear.IServices;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using DangDucThuanFinalYear.Data.Entities;
using DangDucThuanFinalYear.Components.Pages;
using DangDucThuanFinalYear.HotelDTO.RoomTypeDTO;

namespace DangDucThuanFinalYear.Services
{
    public class RoomTypeService : IRoomTypeService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public RoomTypeService(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }




        public async Task<HotelResult<short>> CreateRoomAsync(RoomTypeCreateUpDateDTO input, string userId)
        {
            using var context = _contextFactory.CreateDbContext();
            RoomType? roomType;
            try
            {
                if (input.Id == 0)
                {
                    if (await context.RoomTypes.AnyAsync(x => x.Name == input.Name))
                    {
                        return $" Room Type with the same {input.Name} already exists!";
                    }
                    roomType = new RoomType
                    {
                        Name = input.Name,
                        AddedBy = userId,
                        CreationTime = DateTime.Now,
                        Descripcion = input.Descripcion,
                        ImageUrl = input.ImageUrl,
                        IsActive = true,
                        MaxAults = input.MaxAults,
                        MaxChildren = input.MaxChildren,
                        Price = input.Price,
                    };
                    await context.RoomTypes.AddAsync(roomType);
                }
                else
                {
                    if (await context.RoomTypes.AnyAsync(x => x.Name == input.Name && x.Id != input.Id))
                    {
                        return $" Room Type with the same {input.Name} already exists!";
                    }
                    roomType = await context.RoomTypes.AsTracking().FirstOrDefaultAsync(x => x.Id == input.Id);
                    if (roomType is null)
                    {
                        return $"Invalid request";
                    }
                    roomType.Name = input.Name;
                    roomType.Descripcion = input.Descripcion;
                    roomType.ImageUrl = input.ImageUrl;
                    roomType.IsActive = input.IsActive;
                    roomType.MaxAults = input.MaxAults;
                    roomType.MaxChildren = input.MaxChildren;
                    roomType.Price = input.Price;
                    roomType.LastUpdatedBy = userId;
                    roomType.LastUpdated = DateTime.Now;

                    var romTypeAmenities = await context.RoomTypeAmenitys.Where(x => x.RoomTypeId == input.Id).ToListAsync();
                    context.RoomTypeAmenitys.RemoveRange(romTypeAmenities);


                }
                await context.SaveChangesAsync();

                if (input.Amenities.Length > 0)
                {
                    var roomTypeAmenities = input.Amenities.Select(x => new RoomTypeAmenity
                    {
                        AmenityId = x.Id,
                        RoomTypeId = roomType.Id,
                        Unit = x.Unit,
                    });
                    await context.RoomTypeAmenitys.AddRangeAsync(roomTypeAmenities);
                    await context.SaveChangesAsync();

                }
                return roomType.Id;

            }
            catch(Exception ex)
            {
                return ex.Message;
            }
          
            

        }
        public async Task<SearchListRoomTypeResults[]> GetRoomForManagePageResults()
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.RoomTypes
                .Select(x => new SearchListRoomTypeResults(
                    x.Id,
                    x.Name,
                    x.ImageUrl,
                    x.Price
                    )).ToArrayAsync();

        }
        public async Task<RoomTypeCreateUpDateDTO> GetRoomTypeEditAsync(short id)
        {
            using var context = _contextFactory.CreateDbContext();
            var roomType = await context.RoomTypes
                                          .Include(x => x.RoomTypeAmenitys)
                                          .Where(x => x.Id == id)
                                          .Select(x => new RoomTypeCreateUpDateDTO
                                          {
                                              Name = x.Name,
                                              ImageUrl = x.ImageUrl,
                                              Price = x.Price,
                                              Descripcion = x.Descripcion,
                                              IsActive = x.IsActive,
                                              Id = id,
                                              MaxAults = x.MaxAults,
                                              MaxChildren = x.MaxChildren,
                                              Amenities = x.RoomTypeAmenitys.Select(x => new RoomTypeCreateUpDateDTO.RoomTypeAmenityCreateDTO(x.AmenityId, x.Unit)).ToArray()
                                          }).FirstOrDefaultAsync();
            return roomType;
        }

        public async Task<Data.Entities.Room[]> GetRoomsAllAsync(short roomTypeId)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Rooms.Where(x => x.RoomTypeId == roomTypeId && x.IsDeleted == false).ToArrayAsync();
        }
        public async Task<Data.Entities.Room[]> CheckRoomVaildAsync(short roomTypeId)
        {
            using var context = _contextFactory.CreateDbContext();
            var booking = await context.Boookings.Where(x => x.RoomTypeId == roomTypeId).ToArrayAsync();
            var room = await context.Rooms.Where(x => x.RoomTypeId == roomTypeId && x.IsDeleted == false).ToArrayAsync();
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            var bookedRoomsToday = booking
                .Where(b => b.CheckInDateTime <= today && b.CheckOutDateTime >= today)
                .Select(b => b.RoomId)
                .ToList(); // Chuyển sang List để sử dụng phương thức Contains

            var availableRoomsToday = room
                .Where(r => !bookedRoomsToday.Contains(r.Id)) 
                .ToArray();
            var notavailableRoomsToday = room
              .Where(r => bookedRoomsToday.Contains(r.Id))
              .ToArray();
            foreach (var item in availableRoomsToday)
            {
                item.IsAvaiable = true;
                context.Update(item);
            }
            foreach (var item in notavailableRoomsToday)
            {
                item.IsAvaiable = false;
                context.Update(item);
            }
            await context.SaveChangesAsync();
            var updatedRooms = await context.Rooms
                .Where(x => x.RoomTypeId == roomTypeId && x.IsDeleted == false)
                .ToArrayAsync();


            return updatedRooms;
        }

        public async Task<HotelResult<Data.Entities.Room>> SaveRoomAsync(Data.Entities.Room room)
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                if (room.Id == 0)
                {
                    if(await context.Rooms.AnyAsync(x=>x.RoomNumber == room.RoomNumber))
                    {
                        return "Room number exists alredy.Please try again!";
                    }
                    await context.Rooms.AddAsync(room);
                }
                else
                {
                    var Room = await context.Rooms.AsTracking().FirstOrDefaultAsync(x => x.Id == room.Id && x.IsDeleted == false);
                    if (Room is null)
                    {
                        return "Invaild Request";
                    }
                    Room.IsAvaiable = room.IsAvaiable;
                }
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception : {ex.Message}");
            }
            return room;


        }

        public async Task<HotelResult> DeleteRoomAsync(int roomId)
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                var Room = await context.Rooms.AsTracking().FirstOrDefaultAsync(x => x.Id == roomId);
                if (Room is null)
                {
                    return "Invaild Request";
                }
                Room.IsDeleted = true;
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception : {ex.Message}");
            }
            return true;
        }

        public async Task<HotelResult> AssignRoomToUserAsync(long bookingId, int roomId)
        {
            using var context = _contextFactory.CreateDbContext();
            var roomasgin = await context.Rooms.FirstOrDefaultAsync(x => x.Id == roomId &&!x.IsDeleted);
            if (roomasgin is null)
            {
                return "Invalid Request";
            }
            var booking = await context.Boookings
                                        .AsTracking()
                                        .FirstOrDefaultAsync(x => x.Id == bookingId);
            if (booking is null)
                return "Invaild Request";
            if (booking is not null)
            {
                var bookingcheck = await context.Boookings
                                       .Where(b => b.RoomId == roomId &&
                                                   b.Id != bookingId &&
                                                   ((b.CheckInDateTime >= booking.CheckInDateTime && b.CheckInDateTime < booking.CheckOutDateTime) ||
                                                   (b.CheckOutDateTime > booking.CheckInDateTime && b.CheckOutDateTime <= booking.CheckOutDateTime) ||
                                                   (b.CheckInDateTime <= booking.CheckInDateTime && b.CheckOutDateTime >= booking.CheckOutDateTime)))
                                       .ToArrayAsync();
                var count = bookingcheck.Count();
                if (count > 0)
                {
                    return "Please slect room other.Room booked!";
                }
                else
                {
                    booking.RoomId = roomId;
                    context.Boookings.Update(booking);
                    await context.SaveChangesAsync();
                }    

            }
            return true;




            // Kiểm tra xem booking có tồn tại hay không và trong booking phòng được chọn đã được book chưa và Thời gian checkin và checkout không trùng vào khoảng
            // thời gian book thì vẫn cho book phòng đó nếu thời gian checkin và checkout trùng với thời gian book thì không cho book phòng đó

            //if (booking is not null)
            //{
            //    if (!await context.Boookings.AnyAsync(b => b.RoomId == booking.RoomId &&
            //                                  b.CheckInDateTime > booking.CheckInDateTime &&
            //                                  b.CheckOutDateTime < booking.CheckOutDateTime &&
            //                                  b.Id != bookingId))
            //    {
            //       
            //    }
            //    else
            //    {
            //        return "Invaild Request";

            //    }
            //}

            //if (booking.RoomId.HasValue)
            //{
            //    var roomchange = await context.Rooms
            //        .AsTracking().FirstOrDefaultAsync(x => x.Id == booking.RoomId.Value);
            //    if(roomchange is not null)
            //    {
            //        roomchange.IsAvaiable = true;
            //    }    
            //}
            ////if (booking.CheckInDateTime == DateOnly.FromDateTime(DateTime.Now))
            //roomasgin.IsAvaiable = false;
            //context.Rooms.Update(roomasgin);
            //booking.RoomId = roomId;




        }

        public async Task<string?> GetRoomTypeName(short roomTypeId)
        {
            using var context = _contextFactory.CreateDbContext();
            var roomtye = await context.RoomTypes.FirstOrDefaultAsync(x => x.Id == roomTypeId);
            if (roomtye is not null)
            {
                return await Task.FromResult(roomtye.Name);
            }
            else
            {
                return await Task.FromResult<string?>(null);
            }
        }
    }
    
}
