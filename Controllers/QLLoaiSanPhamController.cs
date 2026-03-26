using LVTNWEBGIAYDEP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LVTNWEBGIAYDEP.Controllers
{
	public class QLLoaiSanPhamController : Controller
	{
        private readonly DBGiayDepContext db;

        public QLLoaiSanPhamController(DBGiayDepContext context)
        {
            db = context;
        }
        public IActionResult Index()
		{
			return View();
		}
		public IActionResult hienThiSanPhamTheoLoai(string tenLoai)
		{
			// Lọc danh sách sản phẩm và nạp thông tin liên quan đến LoaiSanPham và ThuongHieu
			var dssp = db.SanPhams
						 .Include(sp => sp.MaLoaiNavigation)  // Nạp thông tin LoaiSanPham
						 .ThenInclude(loai => loai.MaThNavigation)  // Nạp thông tin ThuongHieu
						 .ToList();

			// Kiểm tra nếu tenLoai không null hoặc rỗng, tiến hành lọc danh sách sản phẩm theo loại
			if (!string.IsNullOrEmpty(tenLoai))
			{
				dssp = dssp.Where(sp => sp.MaLoaiNavigation.TenLoai == tenLoai).ToList();
			}

			// Thêm thông báo vào TempData và ViewBag
			TempData["SuccessMessage"] = tenLoai;
			ViewBag.SuccessMessage = TempData["SuccessMessage"];

			// Truyền danh sách sản phẩm vào View
			return View(dssp);
		}
	}
}
