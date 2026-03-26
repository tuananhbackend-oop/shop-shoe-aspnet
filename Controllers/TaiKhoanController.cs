using LVTNWEBGIAYDEP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LVTNWEBGIAYDEP.Controllers
{
    public class TaiKhoanController : Controller
    {
        private readonly DBGiayDepContext db;

        public TaiKhoanController(DBGiayDepContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            return View(db.TaiKhoans.ToList());
        }
        public IActionResult xoaTaiKhoanhd(string xoatk)
        {
            try
            {
                var tk = db.TaiKhoans.Find(xoatk);
                if (tk != null)
                {
                    
                    db.TaiKhoans.Remove(tk);
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Xoá tài khoản thành công!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Không tìm thấy tài khoản!";
                }
            }
            catch (DbUpdateException dbEx)
            {
                // Ghi lại chi tiết DbUpdateException
                TempData["ErrorMessage"] = $"Tài khoản đã có đơn hàng hoặc liên hệ, không thể xoá!";
            }
           
            return RedirectToAction("Index");
        }
        public IActionResult TrangThaiTaiKhoan(string tk)
        {
            TaiKhoan taikhoan = db.TaiKhoans.Find(tk);
            if (taikhoan != null)
            {
                if (taikhoan.trangThai == "Hoạt Động")
                {
                    taikhoan.trangThai = "Đóng Băng";
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    taikhoan.trangThai = "Hoạt Động";
                    db.SaveChanges();
                    return RedirectToAction("DanhSachTaiKhoanDongBang");
                }
            }
            
            return RedirectToAction("Index");
           
          
        }
        public IActionResult DanhSachTaiKhoanDongBang()
        {
          
            return View(db.TaiKhoans.ToList());
        }
        public IActionResult xoaTaiKhoandb(string xoatk)
        {
            try
            {
                var tk = db.TaiKhoans.Find(xoatk);
                if (tk != null)
                {

                    db.TaiKhoans.Remove(tk);
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Xoá tài khoản thành công!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Không tìm thấy tài khoản!";
                }
            }
            catch (DbUpdateException dbEx)
            {
                // Ghi lại chi tiết DbUpdateException
                TempData["ErrorMessage"] = $"Tài khoản đã có đơn hàng hoặc liên hệ, không thể xoá!";
            }

            return RedirectToAction("DanhSachTaiKhoanDongBang");
        }
        public IActionResult PhanQuyen(string tk, string pq)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    // Lấy các chi tiết đơn hàng theo ID đơn hàng
                   
                    var tkhoan = db.TaiKhoans.Find(tk);
                    

                        if (tkhoan != null)
                        {
                        tkhoan.phanQuyen = pq;
                        }
                        db.SaveChanges();
                        transaction.Commit();
                        // Hoàn trả số lượng vào hàng tồn kho

                }
                catch (Exception ex)
                {
                    // Rollback nếu có lỗi
                    transaction.Rollback();

                }
            }

            // Quay lại trang danh sách hóa đơn
            return RedirectToAction("Index");
        }


        public IActionResult capNhatMK(string tk, string newMatKhau, string oldMatKhau)
        {
            // Kiểm tra tài khoản và mật khẩu cũ
            TaiKhoan tkhoan = db.TaiKhoans.Find(tk);
            if (tkhoan != null )
            {
                tkhoan.MatKhau = BCrypt.Net.BCrypt.HashPassword(newMatKhau);// Cập nhật mật khẩu mới
                db.SaveChanges();
                ViewBag.SuccessMessage = "Cập nhật mật khẩu thành công!";
            }
            else
            {
                ViewBag.ErrorMessage = "Mật khẩu cũ không đúng!";
            }

            return RedirectToAction("Index");  // Quay lại trang Index
        }












    }
}
