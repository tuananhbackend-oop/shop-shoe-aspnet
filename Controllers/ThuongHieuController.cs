using LVTNWEBGIAYDEP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LVTNWEBGIAYDEP.Controllers
{
	public class ThuongHieuController : Controller
	{
        private readonly DBGiayDepContext db;

        public ThuongHieuController(DBGiayDepContext context)
        {
            db = context;
        }
        public IActionResult Index()
		{
            List<ThuongHieu> th = db.ThuongHieus.ToList();
      
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            return View(db.ThuongHieus.ToList());
		}
	
        public IActionResult formthemThuongHieu()
        {
            return View();
        }
        public IActionResult themThuongHieu(string TenTh)
        {
            var thuongHieu = new ThuongHieu()
            {
                TenTh = TenTh,
            };
			db.ThuongHieus.Add(thuongHieu);
			db.SaveChanges();
            TempData["SuccessMessage"] = "Thêm thương hiệu thành công!";
            return RedirectToAction("Index");
        }

        public IActionResult xoaThuongHieu(int xoaid)
        {
            try
            {
                // Thay đổi từ LoaiSanPhams thành ThuongHieus
                var th = db.ThuongHieus.Find(xoaid);  // Sửa từ db.LoaiSanPhams.Find thành db.ThuongHieus.Find
                if (th != null)
                {
                    db.ThuongHieus.Remove(th);  // Sửa từ db.LoaiSanPhams.Remove thành db.ThuongHieus.Remove
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Xoá thương hiệu thành công!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Không tìm thấy thương hiệu!";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Thương hiệu đã có loại sản phẩm, không thể xoá";
            }

            return RedirectToAction("Index"); // Điều hướng lại trang
        }
        public IActionResult formSuaTH(int id)
        {
            ThuongHieu x = db.ThuongHieus.Find(id);
            return View(x);
        }
        public IActionResult suaTH(ThuongHieu hieu)
        {
            db.ThuongHieus.Update(hieu);
            db.SaveChanges();
            TempData["SuccessMessage"] = "Sửa thương hiệu thành công!";
            return RedirectToAction("Index");
        }
        public IActionResult loaiSanPham()
        {

          
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            return View();

        }
        public IActionResult formThemLoaiSanPham()
        {
          
            ViewBag.DSTH = new SelectList(db.ThuongHieus.ToList(), "MaTh", "TenTh");
            return View();
        }
        public IActionResult themLoaiSP(LoaiSanPham loaiSanPham)
        {
            var lp = db.LoaiSanPhams.Where(x => x.MaLoai == loaiSanPham.MaLoai);
            if (lp != null)
            {


                db.LoaiSanPhams.Add(loaiSanPham);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Thêm loại sản phẩm thành công!";
                return RedirectToAction("LoaiSanPham");
            }
            else
            {
                return RedirectToAction("LoaiSanPham");
            }
         
        }
        public IActionResult xoaLoaiSP(int xoasp)
        {
            try
            {
                var sp = db.LoaiSanPhams.Find(xoasp);
                if (sp != null)
                {
                    db.LoaiSanPhams.Remove(sp);
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Xoá loại sản phẩm thành công!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Không tìm thấy loại sản phẩm!";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Loại sản phẩm đã tạo sản phẩm, không thể xoá";
            }

            // Redirect lại trang LoaiSanPham
            return RedirectToAction("LoaiSanPham");
        }
        public IActionResult formSuaLoaiSP(int idsua)
        {
            ViewBag.DSTH = new SelectList(db.ThuongHieus.ToList(), "MaTh" , "TenTh");
            LoaiSanPham loai = db.LoaiSanPhams.Find(idsua);
         
            return View(loai);

        }
        public IActionResult suaLoaiSP(LoaiSanPham lsp)
        {
            db.LoaiSanPhams.Update(lsp);
            db.SaveChanges();
            TempData["SuccessMessage"] = "Sửa loại sản phẩm thành công!";
            return RedirectToAction("LoaiSanPham");

        }
		












	}
}
