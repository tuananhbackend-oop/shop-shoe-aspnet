using Humanizer.Localisation;
using LVTNWEBGIAYDEP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LVTNWEBGIAYDEP.Controllers
{
	public class AdminController : Controller
	{
        private readonly DBGiayDepContext db;

        public AdminController(DBGiayDepContext context)
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
                if(item.HangTonKho == 0)
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



        public IActionResult themSanPham(string TenSp, IFormFile fileImg, string TrangThai, string MoTa,
         int MaLoai, DateTime Ngaythemsp,
         int size39, int size40, int size41, int size42, int size43, int size44, int HangTonKho39, 
         int HangTonKho40, int HangTonKho41, int HangTonKho42, int HangTonKho43, int HangTonKho44,
        int GiaBan39, int GiaBan40, int GiaBan41, int GiaBan42, int GiaBan43, int GiaBan44)
        {
                if (size39 != 0)
                {
                
                string maSp1 = TenSp.Substring(0, 1).ToUpper() + GiaBan39.ToString().Substring(0, 1).ToUpper() + TrangThai.Substring(0, 1).ToUpper() + MoTa.Substring(0, 1).ToUpper()
                    + MaLoai.ToString() + size39.ToString();
                if (db.SanPhams.Any(sp => sp.MaSp == maSp1))
                {
                    maSp1 = $"{maSp1}_{Guid.NewGuid()}"; // Đảm bảo mã sản phẩm duy nhất
                }
                var existingEntry = db.ChangeTracker.Entries<SanPham>().FirstOrDefault(e => e.Entity.MaSp == maSp1);
                if (existingEntry != null)
                {
                    db.Entry(existingEntry.Entity).State = EntityState.Detached;
                }
                SanPham sp1 = new SanPham();
                    sp1.MaSp = maSp1;
                    sp1.TenSp = TenSp;
                    if (fileImg == null)
                    {
                        sp1.HinhAnh = string.Empty;

                    }
                    else
                    {
                        sp1.HinhAnh = fileImg.FileName;
                        string tenfile = Directory.GetCurrentDirectory();
                        tenfile += @"\wwwroot\HinhAnh\" + fileImg.FileName;

                            FileStream f = new FileStream(tenfile, FileMode.Create);
                        fileImg.CopyTo(f);
                        f.Close();
                    }
                    sp1.GiaBan = GiaBan39;
                    sp1.TrangThai = TrangThai;
                    sp1.NgayThemSP = Ngaythemsp;
                    sp1.MoTa = MoTa;
                    sp1.HangTonKho = HangTonKho39;
                    sp1.KichCo = size39;
                    sp1.MaLoai = MaLoai;

                    db.SanPhams.Add(sp1);
                db.SaveChanges();
            }
             if (size40 != 0)
            {
                string maSp2 = TenSp.Substring(0, 1).ToUpper() + GiaBan40.ToString().Substring(0, 1).ToUpper() + TrangThai.Substring(0, 1).ToUpper() + MoTa.Substring(0, 1).ToUpper()
                   + MaLoai.ToString() + size40.ToString();
                if (db.SanPhams.Any(sp => sp.MaSp == maSp2))
                {
                    maSp2 = $"{maSp2}_{Guid.NewGuid()}"; // Đảm bảo mã sản phẩm duy nhất
                }
                var existingEntry = db.ChangeTracker.Entries<SanPham>().FirstOrDefault(e => e.Entity.MaSp == maSp2);
                if (existingEntry != null)
                {
                    db.Entry(existingEntry.Entity).State = EntityState.Detached;
                }
                SanPham sp = new SanPham();
                sp.MaSp = maSp2;
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
                sp.GiaBan = GiaBan40;
                sp.TrangThai = TrangThai;
                sp.NgayThemSP = Ngaythemsp;
                sp.MoTa = MoTa;
                sp.HangTonKho = HangTonKho40;
                sp.KichCo = size40;
                sp.MaLoai = MaLoai;

                db.SanPhams.Add(sp);
                db.SaveChanges();
            }
           if (size41 != 0)
            {
                string maSp3 = TenSp.Substring(0, 1).ToUpper() + GiaBan41.ToString().Substring(0, 1).ToUpper() + TrangThai.Substring(0, 1).ToUpper() + MoTa.Substring(0, 1).ToUpper()
                   + MaLoai.ToString() + size41.ToString();
                if (db.SanPhams.Any(sp => sp.MaSp == maSp3))
                {
                    maSp3 = $"{maSp3}_{Guid.NewGuid()}"; // Đảm bảo mã sản phẩm duy nhất
                }
                var existingEntry = db.ChangeTracker.Entries<SanPham>().FirstOrDefault(e => e.Entity.MaSp == maSp3);
                if (existingEntry != null)
                {
                    db.Entry(existingEntry.Entity).State = EntityState.Detached;
                }
                SanPham sp2 = new SanPham();
                sp2.MaSp = maSp3;
                sp2.TenSp = TenSp;
                if (fileImg == null)
                {
                    sp2.HinhAnh = string.Empty;

                }
                else
                {
                    sp2.HinhAnh = fileImg.FileName;
                    string tenfile = Directory.GetCurrentDirectory();
                    tenfile += @"\wwwroot\HinhAnh\" + fileImg.FileName;

                    FileStream f = new FileStream(tenfile, FileMode.Create);
                    fileImg.CopyTo(f);
                    f.Close();
                }
                sp2.GiaBan = GiaBan41;
                sp2.TrangThai = TrangThai;
                sp2.NgayThemSP = Ngaythemsp;
                sp2.MoTa = MoTa;
                sp2.HangTonKho = HangTonKho41;
                sp2.KichCo = size41;
                sp2.MaLoai = MaLoai;

                db.SanPhams.Add(sp2);
                db.SaveChanges();
            }
             if (size42 != 0)
            {
                string maSp4 = TenSp.Substring(0, 1).ToUpper() + GiaBan42.ToString().Substring(0, 1).ToUpper() + TrangThai.Substring(0, 1).ToUpper() + MoTa.Substring(0, 1).ToUpper()
                   + MaLoai.ToString() + size42.ToString();
                if (db.SanPhams.Any(sp => sp.MaSp == maSp4))
                {
                    maSp4 = $"{maSp4}_{Guid.NewGuid()}"; // Đảm bảo mã sản phẩm duy nhất
                }
                var existingEntry = db.ChangeTracker.Entries<SanPham>().FirstOrDefault(e => e.Entity.MaSp == maSp4);
                if (existingEntry != null)
                {
                    db.Entry(existingEntry.Entity).State = EntityState.Detached;
                }
                SanPham sp3 = new SanPham();
                sp3.MaSp = maSp4;
                sp3.TenSp = TenSp;
                if (fileImg == null)
                {
                    sp3.HinhAnh = string.Empty;

                }
                else
                {
                    sp3.HinhAnh = fileImg.FileName;
                    string tenfile = Directory.GetCurrentDirectory();
                    tenfile += @"\wwwroot\HinhAnh\" + fileImg.FileName;

                    FileStream f = new FileStream(tenfile, FileMode.Create);
                    fileImg.CopyTo(f);
                    f.Close();
                }
                sp3.GiaBan = GiaBan42;
                sp3.TrangThai = TrangThai;
                sp3.NgayThemSP = Ngaythemsp;
                sp3.MoTa = MoTa;
                sp3.HangTonKho = HangTonKho42;
                sp3.KichCo = size42;
                sp3.MaLoai = MaLoai;

                db.SanPhams.Add(sp3);
                db.SaveChanges();
            }
             if (size43 != 0)
            {
                string maSp5 = TenSp.Substring(0, 1).ToUpper() + GiaBan43.ToString().Substring(0, 1).ToUpper() + TrangThai.Substring(0, 1).ToUpper() + MoTa.Substring(0, 1).ToUpper()
                   + MaLoai.ToString() + size43.ToString();
                if (db.SanPhams.Any(sp => sp.MaSp == maSp5))
                {
                    maSp5 = $"{maSp5}_{Guid.NewGuid()}"; // Đảm bảo mã sản phẩm duy nhất
                }
                var existingEntry = db.ChangeTracker.Entries<SanPham>().FirstOrDefault(e => e.Entity.MaSp == maSp5);
                if (existingEntry != null)
                {
                    db.Entry(existingEntry.Entity).State = EntityState.Detached;
                }
                SanPham sp4 = new SanPham();
                sp4.MaSp = maSp5;
                sp4.TenSp = TenSp;
                if (fileImg == null)
                {
                    sp4.HinhAnh = string.Empty;

                }
                else
                {
                    sp4.HinhAnh = fileImg.FileName;
                    string tenfile = Directory.GetCurrentDirectory();
                    tenfile += @"\wwwroot\HinhAnh\" + fileImg.FileName;

                    FileStream f = new FileStream(tenfile, FileMode.Create);
                    fileImg.CopyTo(f);
                    f.Close();
                }
                sp4.GiaBan = GiaBan43;
                sp4.TrangThai = TrangThai;
                sp4.NgayThemSP = Ngaythemsp;
                sp4.MoTa = MoTa;
                sp4.HangTonKho = HangTonKho43;
                sp4.KichCo = size43;
                sp4.MaLoai = MaLoai;

                db.SanPhams.Add(sp4);
                db.SaveChanges();
            }
            if (size44 != 0)
            {
                string maSp6 = TenSp.Substring(0, 1).ToUpper() + GiaBan44.ToString().Substring(0, 1).ToUpper() + TrangThai.Substring(0, 1).ToUpper() + MoTa.Substring(0, 1).ToUpper()
                   + MaLoai.ToString() + size44.ToString();
                if (db.SanPhams.Any(sp => sp.MaSp == maSp6))
                {
                    maSp6 = $"{maSp6}_{Guid.NewGuid()}"; // Đảm bảo mã sản phẩm duy nhất
                }
                var existingEntry = db.ChangeTracker.Entries<SanPham>().FirstOrDefault(e => e.Entity.MaSp == maSp6);
                if (existingEntry != null)
                {
                    db.Entry(existingEntry.Entity).State = EntityState.Detached;
                }
                SanPham sp5 = new SanPham();
                sp5.MaSp = maSp6;
                sp5.TenSp = TenSp;
                if (fileImg == null)
                {
                    sp5.HinhAnh = string.Empty;

                }
                else
                {
                    sp5.HinhAnh = fileImg.FileName;
                    string tenfile = Directory.GetCurrentDirectory();
                    tenfile += @"\wwwroot\HinhAnh\" + fileImg.FileName;

                    FileStream f = new FileStream(tenfile, FileMode.Create);
                    fileImg.CopyTo(f);
                    f.Close();
                }
                sp5.GiaBan = GiaBan44;
                sp5.TrangThai = TrangThai;
                sp5.NgayThemSP = Ngaythemsp;
                sp5.MoTa = MoTa;
                sp5.HangTonKho = HangTonKho44;
                sp5.KichCo = size44;
                sp5.MaLoai = MaLoai;

                db.SanPhams.Add(sp5);
                db.SaveChanges();
            }
   
            TempData["SuccessMessage"] = "Thêm sản phẩm thành công!";
            return RedirectToAction("SanPham");


        }







        public IActionResult xemChiTietSanPham(string ten)
        {
            // Lấy danh sách sản phẩm theo tên với Eager Loading
        var Sanp =db.SanPhams.Include(sp=>sp.MaLoaiNavigation).ThenInclude(sp1=>sp1.MaThNavigation).Where(t=>t.TenSp == ten).ToList();
        
            return View(Sanp);
        }
        public IActionResult xoaHangHoa(string xoaten)
        {
            using var transaction = db.Database.BeginTransaction();
            try
            {
              
               

                // Xóa dữ liệu liên quan (ví dụ: OrderDetails)
                var relatedOrders = db.SanPhams.Where(o => o.TenSp == xoaten).ToList();
                if (relatedOrders == null)
                {
                    TempData["ErrorMessage"] = "Sản phẩm không tồn tại.";
                    return RedirectToAction("SanPham");
                }
         

                // Xóa sản phẩm
                db.SanPhams.RemoveRange(relatedOrders);
                db.SaveChanges();

                transaction.Commit();
                TempData["SuccessMessage"] = "Xoá sản phẩm thành công!";
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                TempData["ErrorMessage"] = "Sản phẩm này đang có đơn hàng không thể xoá";
            }

            return RedirectToAction("SanPham");
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
         

            return View(sp);  // Trả về sản phẩm để hiển thị thông tin sửa
        }

        public IActionResult suaSP(SanPham sps)
        {
           db.SanPhams.Update(sps);
            db.SaveChanges();
            TempData["SuccessMessage"] = "Sửa sản phẩm thành công!";
            return RedirectToAction("SanPham");
        }
        public IActionResult xoaCtHangHoa(string xoaid)
        {
            using var transaction = db.Database.BeginTransaction();
            try
            {
                // Tìm sản phẩm theo MaSp
                var relatedOrders = db.SanPhams.Find(xoaid);

                if (relatedOrders == null)
                {
                    TempData["ErrorMessage"] = "Sản phẩm không tồn tại.";
                    return RedirectToAction("SanPham");
                }

                // Kiểm tra nếu sản phẩm có đơn hàng liên quan
                if (relatedOrders.ChiTietDonHangs.Any())
                {
                    TempData["ErrorMessage"] = "Sản phẩm này đang có đơn hàng không thể xoá";
                    return RedirectToAction("SanPham");
                }

                // Xóa sản phẩm nếu không có đơn hàng liên quan
                db.SanPhams.Remove(relatedOrders);
                db.SaveChanges();

                transaction.Commit();
                TempData["SuccessMessage"] = "Xóa sản phẩm thành công!";
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi xóa sản phẩm: " + ex.Message;
            }

            return RedirectToAction("SanPham");
        }




    }
}
