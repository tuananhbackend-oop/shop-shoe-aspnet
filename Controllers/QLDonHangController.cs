using LVTNWEBGIAYDEP.Models;
using Microsoft.AspNetCore.Mvc;

namespace LVTNWEBGIAYDEP.Controllers
{
	public class QLDonHangController : Controller
	{
        private readonly DBGiayDepContext db;

        public QLDonHangController(DBGiayDepContext context)
        {
            db = context;
        }
        public IActionResult xemDanhSachHoaDon()
		{
            List<DonHang> dshd = db.DonHangs.ToList();
            foreach (var item in dshd)
            {
                item.TaiKhoanNavigation = db.TaiKhoans.Find(item.TaiKhoan);
            }
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
          
			return View(db.DonHangs.ToList());
		}
        public IActionResult xemDanhSachHoaDonHuy()
        {
            List<DonHang> dshd = db.DonHangs.ToList();
            foreach (var item in dshd)
            {
                item.TaiKhoanNavigation = db.TaiKhoans.Find(item.TaiKhoan);
            }
            ViewBag.SuccessMessage = TempData["SuccessMessage"];

            return View(db.DonHangs.ToList());
        }
		public IActionResult xemDanhSachHoaDonHoanTat()
		{
			List<DonHang> dshd = db.DonHangs.ToList();
			foreach (var item in dshd)
			{
				item.TaiKhoanNavigation = db.TaiKhoans.Find(item.TaiKhoan);
			}
			

			return View(db.DonHangs.ToList());
		}
		public IActionResult hienThiChiTietHD(int id)
        {
            DonHang dh = db.DonHangs.Find(id);

            dh.ChiTietDonHangs = db.ChiTietDonHangs.Where(t => t.iDDonHang == id).ToList();
            foreach (ChiTietDonHang a in dh.ChiTietDonHangs)
            {
                a.MaSpNavigation = db.SanPhams.Find(a.MaSp);
            }
            return PartialView(dh);
        }
		public IActionResult hienThiChiTietHDHuy(int id)
		{
			DonHang dh = db.DonHangs.Find(id);

			dh.ChiTietDonHangs = db.ChiTietDonHangs.Where(t => t.iDDonHang == id).ToList();
			foreach (ChiTietDonHang a in dh.ChiTietDonHangs)
			{
				a.MaSpNavigation = db.SanPhams.Find(a.MaSp);
			}
			return PartialView(dh);
		}
		public IActionResult hienThiChiTietHDHoanTat(int id)
		{
			DonHang dh = db.DonHangs.Find(id);

			dh.ChiTietDonHangs = db.ChiTietDonHangs.Where(t => t.iDDonHang == id).ToList();
			foreach (ChiTietDonHang a in dh.ChiTietDonHangs)
			{
				a.MaSpNavigation = db.SanPhams.Find(a.MaSp);
			}
			return PartialView(dh);
		}
		public IActionResult xoaDonHang(int idhd)
		{
			try
			{
				// Tìm kiếm các chi tiết đơn hàng theo ID đơn hàng
				var chiTietDonHangs = db.ChiTietDonHangs.Where(ctdh => ctdh.iDDonHang == idhd).ToList();

				// Xóa các chi tiết đơn hàng
				db.ChiTietDonHangs.RemoveRange(chiTietDonHangs);

				// Lưu thay đổi vào cơ sở dữ liệu
				db.SaveChanges();

				// Tìm kiếm và xóa đơn hàng
				DonHang dh = db.DonHangs.Find(idhd);
				if (dh != null)
				{
					db.DonHangs.RemoveRange(dh);
					db.SaveChanges();
				}
				TempData["SuccessMessage"] = "Xoá đơn hàng thành công !";
			}
			catch (Exception ex)
			{
				TempData["ErrorMessage"] = "Không thể xoá";
			}

			// Quay lại trang danh sách hóa đơn
			return RedirectToAction("xemDanhSachHoaDon");
		}
		public IActionResult xoaDonHangHuy(int idhd)
        {
            try
            {
                // Tìm kiếm các chi tiết đơn hàng theo ID đơn hàng
                var chiTietDonHangs = db.ChiTietDonHangs.Where(ctdh => ctdh.iDDonHang == idhd).ToList();

                // Xóa các chi tiết đơn hàng
                db.ChiTietDonHangs.RemoveRange(chiTietDonHangs);

                // Lưu thay đổi vào cơ sở dữ liệu
                db.SaveChanges();

                // Tìm kiếm và xóa đơn hàng
                DonHang dh = db.DonHangs.Find(idhd);
                if (dh != null)
                {
                    db.DonHangs.RemoveRange(dh);
                    db.SaveChanges();
                }
                TempData["SuccessMessage"] = "Xoá đơn hàng thành công !";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Không thể xoá";
            }

            // Quay lại trang danh sách hóa đơn
            return RedirectToAction("xemDanhSachHoaDonHuy");
        }
		public IActionResult xoaDonHangHoanTat(int idhd)
		{
			try
			{
				// Tìm kiếm các chi tiết đơn hàng theo ID đơn hàng
				var chiTietDonHangs = db.ChiTietDonHangs.Where(ctdh => ctdh.iDDonHang == idhd).ToList();

				// Xóa các chi tiết đơn hàng
				db.ChiTietDonHangs.RemoveRange(chiTietDonHangs);

				// Lưu thay đổi vào cơ sở dữ liệu
				db.SaveChanges();

				// Tìm kiếm và xóa đơn hàng
				DonHang dh = db.DonHangs.Find(idhd);
				if (dh != null)
				{
					db.DonHangs.RemoveRange(dh);
					db.SaveChanges();
				}
				TempData["SuccessMessage"] = "Xoá đơn hàng thành công !";
			}
			catch (Exception ex)
			{
				TempData["ErrorMessage"] = "Không thể xoá";
			}

			// Quay lại trang danh sách hóa đơn
			return RedirectToAction("xemDanhSachHoaDonHoanTat");
		}
		public IActionResult xemDanhSachHoaDonKhongNhan()
		{
			List<DonHang> dshd = db.DonHangs.ToList();
			foreach (var item in dshd)
			{
				item.TaiKhoanNavigation = db.TaiKhoans.Find(item.TaiKhoan);
			}


			return View(db.DonHangs.ToList());
		}
		public IActionResult hienThiChiTietHDKhongNhan(int id)
		{
			DonHang dh = db.DonHangs.Find(id);

			dh.ChiTietDonHangs = db.ChiTietDonHangs.Where(t => t.iDDonHang == id).ToList();
			foreach (ChiTietDonHang a in dh.ChiTietDonHangs)
			{
				a.MaSpNavigation = db.SanPhams.Find(a.MaSp);
			}
			return PartialView(dh);
		}
		public IActionResult xoaDonHangKhongNhan(int idhd)
		{
			try
			{
				// Tìm kiếm các chi tiết đơn hàng theo ID đơn hàng
				var chiTietDonHangs = db.ChiTietDonHangs.Where(ctdh => ctdh.iDDonHang == idhd).ToList();

				// Xóa các chi tiết đơn hàng
				db.ChiTietDonHangs.RemoveRange(chiTietDonHangs);

				// Lưu thay đổi vào cơ sở dữ liệu
				db.SaveChanges();

				// Tìm kiếm và xóa đơn hàng
				DonHang dh = db.DonHangs.Find(idhd);
				if (dh != null)
				{
					db.DonHangs.RemoveRange(dh);
					db.SaveChanges();
				}
				TempData["SuccessMessage"] = "Xoá đơn hàng thành công !";
			}
			catch (Exception ex)
			{
				TempData["ErrorMessage"] = "Không thể xoá";
			}

			// Quay lại trang danh sách hóa đơn
			return RedirectToAction("xemDanhSachHoaDonKhongNhan");
		}
		public IActionResult xacNhanDonHang(int iddh)
		{
            DonHang dh = db.DonHangs.Find(iddh);
            if (dh != null)
            {
                if (dh.trangThai == "Chờ xác nhận")
                {
                    dh.trangThai = "Chờ lấy hàng";
                 
                }
                else if (dh.trangThai == "Chờ lấy hàng")
                {
                    dh.trangThai = "Chờ giao hàng";
                  
                }
                else if(dh.trangThai == "Chờ giao hàng")
                {
                    dh.trangThai = "Đã nhận được hàng";
                  
                }
				else if(dh.trangThai == "Đã nhận được hàng")
				{
					dh.trangThai = "Hoàn Tất";
				}	
                db.SaveChanges();
				return RedirectToAction("xemDanhSachHoaDon");

			}
            return RedirectToAction("xemDanhSachHoaDon");
		}




	}
}
