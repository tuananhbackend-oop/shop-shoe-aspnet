namespace LVTNWEBGIAYDEP.Models.Vnpay
{
	public class PaymentInformationModel
	{
        public string OrderId { get; set; }
        public string OrderType { get; set; }
        public double Amount { get; set; }
        public string OrderDescription { get; set; }
        public string Name { get; set; }
        public string? soDienThoai { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

    }
}
