using LVTNWEBGIAYDEP.Models;
using LVTNWEBGIAYDEP.Models.Vnpay;
using LVTNWEBGIAYDEP.Services.Momo;
using LVTNWEBGIAYDEP.Services.Vnpay;
using Microsoft.AspNetCore.Mvc;

namespace LVTNWEBGIAYDEP.Controllers
{
	public class PaymentController : Controller
	{
        private readonly DBGiayDepContext db;
        private readonly IMomoService _momoService;
        private readonly IVnPayService _vnPayService;

        public PaymentController(
            DBGiayDepContext context,
            IMomoService momoService,
            IVnPayService vnPayService)
        {
            db = context;
            _momoService = momoService;
            _vnPayService = vnPayService;
        }
        [HttpPost]
		[Route("CreatePaymentUrl")]
		public async Task<IActionResult> CreatePaymentMoMo(OrderInfo model)
		{
			var isTaiKhoanHopLe = db.TaiKhoans.Any(t => t.TaiKhoan1 == model.FullName);
			if (!isTaiKhoanHopLe)
			{
				// Thêm lỗi vào ModelState
				TempData["SuccessMessagesaitk"] = "Tài khoản không hợp lệ";

				return RedirectToAction("FormGioHang","User");
			}
			
			var response = await _momoService.CreatePaymentMomo(model);
			var requestQuery = HttpContext.Request.Query;
			if (requestQuery["resultCode"] !=0)

			{
                TempData["SuccessMessage"] =model.soDienThoai;
                TempData["SuccessMessage1"] = model.FullName;
				TempData["SuccessMessage4"] = model.OrderInformation;
				TempData["Amount"] = model.Amount.ToString("#,0", new System.Globalization.CultureInfo("vi-VN"));

                return Redirect(response.PayUrl);
			}
			else
			{
				TempData["ErrorMessage"] = "Thanh toán không thành công. Vui lòng thử lại!";
				return RedirectToAction("Error");
			}
		}

		[HttpGet]
		public async Task<IActionResult> PaymentCallBack(OrderInfo model)
		{
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.SuccessMessage1 = TempData["SuccessMessage1"];
			ViewBag.SuccessMessage3 = TempData["Amount"];
			ViewBag.SuccessMessage4 = TempData["SuccessMessage4"];
			var response = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);
            var requestQuery = HttpContext.Request.Query;
			if (requestQuery["resultCode"] != 0)//Giao dịch không thành công
			{
				model.soDienThoai = ViewBag.SuccessMessage;
				model.FullName = ViewBag.SuccessMessage1;
				model.OrderInformation = ViewBag.SuccessMessage4;
                var soDienThoai = model.soDienThoai;
				var taiKhoan = model.FullName;
				var diaChi = model.OrderInformation;
				var donHang = new DonHang
				{
					SoDienThoai = soDienThoai, // Chuyển đổi số điện thoại từ string sang int
					TaiKhoan = taiKhoan,
					DiaChi = diaChi,
					thanhToan = "Đã thanh toán bằng MoMo",
					trangThai = "Chờ xác nhận",
					ngayLapDon=DateTime.Now,
				};
				DonHang hd = MySessions.Get<DonHang>(HttpContext.Session, "donhang");

				// Thêm chi tiết đơn hàng
				foreach (ChiTietDonHang a in hd.ChiTietDonHangs)
				{
					// Đảm bảo dữ liệu không bị null
					if (a.MaSp == null || a.SoLuong <= 0)
						continue;

					ChiTietDonHang ct = new ChiTietDonHang
					{
						MaSp = a.MaSp,
						SoLuong = a.SoLuong,
						iDDonHang = donHang.iDDonHang
					};
					donHang.ChiTietDonHangs.Add(ct);
				}
				db.DonHangs.Add(donHang);
				foreach (ChiTietDonHang chiTiet in donHang.ChiTietDonHangs)
				{
					// Lấy sản phẩm từ database
					var sanPham = db.SanPhams.FirstOrDefault(sp => sp.MaSp == chiTiet.MaSp);
					if (sanPham != null)
					{
						// Giảm số lượng tồn kho
						sanPham.HangTonKho -= chiTiet.SoLuong;

						// Đảm bảo số lượng tồn kho không âm

					}
				}
				db.SaveChanges();

				// Reset lại session
				MySessions.Set<DonHang>(HttpContext.Session, "donhang", new DonHang());
				
			}
			else
			{
				TempData["ErrorMessage"] = "Bạn huỷ giao dịch momo";
				return RedirectToAction("FormGioHang","User");
            }
			return View(response);
           
		}
		public IActionResult CreatePaymentUrlVnpay(PaymentInformationModel model)
		{
            var isTaiKhoanHopLe = db.TaiKhoans.Any(t => t.TaiKhoan1 == model.Name);
            if (!isTaiKhoanHopLe)
            {
                // Thêm lỗi vào ModelState
                TempData["SuccessMessagesaitk"] = "Tài khoản không hợp lệ";

                return RedirectToAction("FormGioHang", "User");
            }
            TempData["SuccessMessagename"] = model.Name;
            TempData["SuccessMessagesodienthoai"] = model.soDienThoai; 
            TempData["SuccessMessagediachi"] = model.OrderDescription;        
            TempData["Amountvnpay"] = model.Amount.ToString("#,0", new System.Globalization.CultureInfo("vi-VN"));
            var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

			return Redirect(url);
		}

		[HttpGet]
		public IActionResult PaymentCallbackVnpay()
		{
			ViewBag.SuccessMessagediachi = TempData["SuccessMessagediachi"];
			ViewBag.SuccessMessagename = TempData["SuccessMessagename"];
            ViewBag.Amountvnpay = TempData["Amountvnpay"];
			ViewBag.SuccessMessagesodienthoai = TempData["SuccessMessagesodienthoai"];
            var response = _vnPayService.PaymentExecute(Request.Query);
			
            if (response.VnPayResponseCode == "00")//Giao dịch không thành công
            {
				var sodienthoai = ViewBag.SuccessMessagesodienthoai;
                var taikhoan = ViewBag.SuccessMessagename;
				var diachi = ViewBag.SuccessMessagediachi;

                var donhang = new DonHang
                {
                    SoDienThoai = sodienthoai, // Chuyển đổi số điện thoại từ string sang int
                    TaiKhoan = taikhoan,
                    DiaChi = diachi,
                    thanhToan = "Đã thanh toán bằng VNPAY",
                    trangThai = "Chờ xác nhận",
					ngayLapDon = DateTime.Now,
                };

                DonHang hd = MySessions.Get<DonHang>(HttpContext.Session, "donhang");

                // Thêm chi tiết đơn hàng
                foreach (ChiTietDonHang a in hd.ChiTietDonHangs)
                {
                    // Đảm bảo dữ liệu không bị null
                    if (a.MaSp == null || a.SoLuong <= 0)
                        continue;

                    ChiTietDonHang ct = new ChiTietDonHang
                    {
                        MaSp = a.MaSp,
                        SoLuong = a.SoLuong,
                        iDDonHang = donhang.iDDonHang
                    };
                    donhang.ChiTietDonHangs.Add(ct);
                }
                db.DonHangs.Add(donhang);
                foreach (ChiTietDonHang chiTiet in donhang.ChiTietDonHangs)
                {
                    // Lấy sản phẩm từ database
                    var sanPham = db.SanPhams.FirstOrDefault(sp => sp.MaSp == chiTiet.MaSp);
                    if (sanPham != null)
                    {
                        // Giảm số lượng tồn kho
                        sanPham.HangTonKho -= chiTiet.SoLuong;

                        // Đảm bảo số lượng tồn kho không âm

                    }
                }
                db.SaveChanges();

                // Reset lại session
                MySessions.Set<DonHang>(HttpContext.Session, "donhang", new DonHang());

            }
            else
            {
                TempData["ErrorMessage"] = "Bạn huỷ giao dịch vnpay";
                return RedirectToAction("FormGioHang", "User");
            }
            return View(response);

        }






    }

}
