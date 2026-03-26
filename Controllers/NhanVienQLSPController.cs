using LVTNWEBGIAYDEP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LVTNWEBGIAYDEP.Controllers
{
    public class NhanVienQLSPController : Controller
    {
        private readonly DBGiayDepContext db;

        public NhanVienQLSPController(DBGiayDepContext context)
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

        public IActionResult formThemSanPham()
        {
           

            // Nhóm dữ liệu theo tên loại sản phẩm
            var groupedData = db.LoaiSanPhams
                .Include(lsp => lsp.MaThNavigation)
                .GroupBy(lsp => lsp.TenLoai) // Nhóm theo tên loại sản phẩm
                .Select(g => new
                {
                    TenLoai = g.Key,
                    MaLoaiList = g.Select(lsp => new { lsp.MaLoai, lsp.MaThNavigation.TenTh }).ToList()
                })
                .ToList();

            // Truyền dữ liệu nhóm sang View
            ViewBag.GroupedLoaiSanPham = groupedData;

            return View();
        }
        public IActionResult themSanPham(string MaSp, string TenSp, IFormFile fileImg, int GiaBan, string TrangThai, string MoTa,
          int HangTonKho, int KichCo, int MaLoai)
        {
            if (string.IsNullOrEmpty(MaSp))
            {
                return View("formThemSanPham");
            }

            SanPham sp = new SanPham();
            sp.MaSp = MaSp;
            sp.TenSp = TenSp;
            if (fileImg == null)
            {
                sp.HinhAnh = string.Empty;

            }
            else
            {
                sp.HinhAnh = fileImg.FileName;
                string tenfile = Directory.GetCurrentDirectory();
                tenfile += @"\wwwroot\HinhAnh\" + fileImg.FileName;

                FileStream f = new FileStream(tenfile, FileMode.Create);
                fileImg.CopyTo(f);
                f.Close();

            }
            sp.GiaBan = GiaBan;
            sp.TrangThai = TrangThai;
            sp.MoTa = MoTa;
            sp.HangTonKho = HangTonKho;
            sp.KichCo = KichCo;
            sp.MaLoai = MaLoai;
        
            db.SanPhams.Add(sp);
            db.SaveChanges();
            TempData["SuccessMessage"] = "Thêm sản phẩm thành công!";
            return RedirectToAction("SanPham");

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
        public IActionResult xoaHangHoa(string id)
        {
            try
            {
                SanPham a = db.SanPhams.Find(id);
                if (a == null)
                {
                    return Json(false);
                }
                else
                {
                    db.SanPhams.Remove(a);
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Xoá sản phẩm thành công!";
                    return Json(true);
                }
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Có lỗi khi xóa sản phẩm!";
                return Json(false);
            }
        }
        public IActionResult formSuaSP(string idsua)
        {
            SanPham sp = db.SanPhams.Find(idsua);
          

            // Lấy thông tin sản phẩm cần sửa


            // Lấy dữ liệu nhóm theo tên loại sản phẩm và thương hiệu
            var groupedData = db.LoaiSanPhams
          .Include(lsp => lsp.MaThNavigation)
          .GroupBy(lsp => lsp.TenLoai) // Nhóm theo tên loại sản phẩm
          .Select(g => new
          {
              TenLoai = g.Key,
              MaLoaiList = g.Select(lsp => new { lsp.MaLoai, lsp.MaThNavigation.TenTh }).ToList()
          })
          .ToList();

            // Truyền dữ liệu nhóm sang View
            ViewBag.GroupedLoaiSanPham = groupedData;

            // Truyền dữ liệu nhóm sang View


            return View(sp);
        }
        public IActionResult suaSP(SanPham sps)
        {
            db.SanPhams.Update(sps);
            db.SaveChanges();
            TempData["SuccessMessage"] = "Sửa sản phẩm thành công!";
            return RedirectToAction("SanPham");
        }
        //***Loai Sản phẩm + thương hiệu***
        public IActionResult ThuongHieu()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            return View(db.ThuongHieus.ToList());
        }

        public IActionResult formthemThuongHieu()
        {
            return View();
        }
        public IActionResult themThuongHieu(ThuongHieu th)
        {
            db.ThuongHieus.Add(th);
            db.SaveChanges();
            TempData["SuccessMessage"] = "Thêm thương hiệu thành công!";
            return RedirectToAction("ThuongHieu");
        }

        public IActionResult xoaThuongHieu(string xoaid)
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

            return RedirectToAction("ThuongHieu"); // Điều hướng lại trang
        }
        public IActionResult formSuaTH(string id)
        {
            ThuongHieu x = db.ThuongHieus.Find(id);
            return View(x);
        }
        public IActionResult suaTH(ThuongHieu hieu)
        {
            db.ThuongHieus.Update(hieu);
            db.SaveChanges();
            TempData["SuccessMessage"] = "Sửa thương hiệu thành công!";
            return RedirectToAction("ThuongHieu");
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

            db.LoaiSanPhams.Add(loaiSanPham);
            db.SaveChanges();
            TempData["SuccessMessage"] = "Thêm loại sản phẩm thành công!";
            return RedirectToAction("LoaiSanPham");

        }
        public IActionResult xoaLoaiSP(string xoasp)
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
        public IActionResult formSuaLoaiSP(string idsua)
        {
            ViewBag.DSTH = new SelectList(db.ThuongHieus.ToList(), "MaTh", "TenTh");
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
