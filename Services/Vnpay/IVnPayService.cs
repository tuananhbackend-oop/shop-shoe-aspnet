using LVTNWEBGIAYDEP.Models.Vnpay;

namespace LVTNWEBGIAYDEP.Services.Vnpay
{
	public interface IVnPayService
	{
		string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
		PaymentResponseModel PaymentExecute(IQueryCollection collections);

	}
}
