namespace DangDucThuanFinalYear.HotelDTO.DashboardDTO
{
    public class DashboardRespon
    {
        public decimal TotalRevenue { get; set; }
        public decimal TotalChi { get; set; }
        public decimal TotalThu { get; set; }


        public decimal TotalRoom { get; set; }
        public int TotalCustomer { get; set; }
        public int TotalStaff { get; set; }

        public decimal TotalBooking { get; set; }
        //public List<RevenueByRoomType> Items { get; set; }
    }
    public class DashboardResponByRoomType
    {
        public string NameRoomType { get; set; }
        public decimal Revenue { get; set; }
        public int BookingCount { get; set; }
    }
    public class Revenue
    {
        public string Name { get; set; }
        public decimal Money { get; set; }
    }

}
