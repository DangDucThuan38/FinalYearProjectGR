namespace DangDucThuanFinalYear.HotelDTO
{
    public class VNPay
    {
        public int OrderId { get; set; }
        public int vnpayTranId { get; set; }
        public string Status { get; set; }
        public string vnp_ResponseCode { get; set; }
        public string vnp_TransactionStatus { get; set; }
        public float vnp_Amount { get; set; }
        public string displayMsg { get; set; }
        public string displayTmnCode { get; set; }
        public string displayTxnRef { get; set; }
        public string displayVnpayTranNo { get; set; }
        public string displayAmount { get; set; }
        public string displayBankCode { get; set; }
    }
}
