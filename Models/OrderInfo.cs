namespace LVTNWEBGIAYDEP.Models
{
    public class OrderInfo
    {
		public string OrderId { get; set; }
		public string FullName { get; set; }
		public string soDienThoai { get; set; }
		public string OrderInformation {  get; set; }
		public string ResultCode { get; set; }
		public double Amount {  get; set; }
        public DateTime DatePaid { get; set; }
    }
}
