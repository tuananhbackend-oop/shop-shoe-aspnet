using LVTNWEBGIAYDEP.Models;
using LVTNWEBGIAYDEP.Models.Momo;

namespace LVTNWEBGIAYDEP.Services.Momo
{
    public interface IMomoService 
    {
		Task<MomoCreatePaymentResponseModel> CreatePaymentMomo(OrderInfo model);

		MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection);

	}
}
