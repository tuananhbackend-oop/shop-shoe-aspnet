using LVTNWEBGIAYDEP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LVTNWEBGIAYDEP.Controllers
{
    public class NhanVienBanHangController : Controller
    {
        private readonly DBGiayDepContext db;

        public NhanVienBanHangController(DBGiayDepContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SanPham()
        {
            List<SanPham> dssp = db.SanPhams.ToList();
            foreach (SanPham item in dssp)
            {
                if (item.HangTonKho == 0)
                {
                    item.TrangThai = "Hết Sản Phẩm";
                }
              
                item.MaLoaiNavigation = db.LoaiSanPhams.Find(item.MaLoai);
                List<LoaiSanPham> dsloai = db.LoaiSanPhams.ToList();
                foreach (LoaiSanPham loai in dsloai)
                {
                    loai.MaThNavigation = db.ThuongHieus.Find(loai.MaTh);
                }
            }
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];

            return View(dssp);
        }

        public IActionResult xemChiTietSanPham(string id)
        {
            List<SanPham> dssp = db.SanPhams.ToList();
            foreach (SanPham item in dssp)
            {

            
                item.MaLoaiNavigation = db.LoaiSanPhams.Find(item.MaLoai);
                List<LoaiSanPham> dsloai = db.LoaiSanPhams.ToList();
                foreach (LoaiSanPham loai in dsloai)
                {
                    loai.MaThNavigation = db.ThuongHieus.Find(loai.MaTh);
                }

            }
            SanPham sp = db.SanPhams.Find(id);

            return View(sp);
        }
        //***Loai Sản phẩm + thương hiệu***
        public IActionResult ThuongHieu()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            return View(db.ThuongHieus.ToList());
        }



        public IActionResult loaiSanPham()
        {


            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            return View();

        }

        //***Ban Hang***

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
                else
                {
                    dh.trangThai = "Đã nhận được hàng";

                }
                db.SaveChanges();
                return RedirectToAction("xemDanhSachHoaDon");

            }
            return RedirectToAction("xemDanhSachHoaDon");
        }

    }
   
       

}
