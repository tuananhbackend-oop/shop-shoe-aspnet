using LVTNWEBGIAYDEP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace LVTNWEBGIAYDEP.Controllers
{
    public class UserController : Controller
    {
        private readonly DBGiayDepContext db;

        public UserController(DBGiayDepContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
			ViewBag.SuccessMessage = TempData["SuccessMessage"];
		

			return View();
        }

        public IActionResult SanPham()
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
			ViewBag.SuccessMessagehetSP = TempData["SuccessMessagehetSP"];
			return View(db.SanPhams.ToList());
        }
		public IActionResult chiTietSP(string MaSp)
		{
			List<SanPham> sanp = db.SanPhams.ToList();
			foreach (var item in sanp)
			{
				item.MaLoaiNavigation = db.LoaiSanPhams.Find(item.MaLoai);
                List<LoaiSanPham> loaith = db.LoaiSanPhams.ToList();
				foreach(var lth in  loaith)
				{
					lth.MaThNavigation = db.ThuongHieus.Find(lth.MaTh);
				}
            }
			SanPham spct = db.SanPhams.Find(MaSp);
			if (spct.HangTonKho == 0)
			{
				spct.TrangThai = "Hết Sản Phẩm";
			}
			return View(spct);
		}
		public IActionResult hetSP()
		{
			TempData["SuccessMessagehetSP"] = "Hết Sản Phẩm, Không thể thêm vào giỏ hàng! ";

			return RedirectToAction("SanPham");
		}
		public IActionResult DonHang()
        {
            return View(db.SanPhams.ToList());
        }
        public IActionResult ChiTietSanPham(string MaSp, int soLuong)
        {
            string id = MaSp;
            SanPham sp = db.SanPhams.Find(id);
            if (sp == null)
            {
                return RedirectToAction("SanPham");
            }

            DonHang dh = MySessions.Get<DonHang>(HttpContext.Session, "donhang");
            if (dh == null)
            {
                dh = new DonHang();
            }
            ChiTietDonHang ct = null;

            foreach (ChiTietDonHang a in dh.ChiTietDonHangs)
            {
                if (a.MaSp == MaSp)
                {
                    ct = a;
                    break;
                }

            }
            if (ct == null)
            {
                ct = new ChiTietDonHang();
                //Them hang hoa vao chi tiet hoa don
                ct.MaSpNavigation = sp;
                ct.MaSp = sp.MaSp;
                ct.SoLuong = soLuong;



                //them chi tiet hoa don vao hoa don
                dh.ChiTietDonHangs.Add(ct);


            }
            else
            {
                ct.SoLuong += soLuong;
            }

            MySessions.Set<DonHang>(HttpContext.Session, "donhang", dh);

            return View("FormGioHang", dh);
        }


        public IActionResult FormGioHang()
        {
            DonHang dh = MySessions.Get<DonHang>(HttpContext.Session, "donhang");
            if (dh == null)
                return RedirectToAction("SanPham");
			ViewBag.SuccessMessage = TempData["SuccessMessage"];
			ViewBag.SuccessMessagesaitk = TempData["SuccessMessagesaitk"];
			ViewBag.SuccessMessage =  TempData["ErrorMessage"];
			return View(dh);
        }
        public IActionResult themDonHang(DonHang dhx)
        {
            var isTaiKhoanHopLe = db.TaiKhoans.Any(t => t.TaiKhoan1 == dhx.TaiKhoan);
            if(!isTaiKhoanHopLe)
                 {
				// Thêm lỗi vào ModelState
				TempData["SuccessMessage"] = "Tài khoản không hợp lệ";
			
				return RedirectToAction("FormGioHang");
            }

            if (ModelState.IsValid)
            {
                // Lấy đơn hàng từ Session
                DonHang hd = MySessions.Get<DonHang>(HttpContext.Session, "donhang");
                dhx.thanhToan = "Thanh Toán Khi nhận";
				dhx.trangThai = "Chờ xác nhận";
				dhx.ngayLapDon = DateTime.Now;

                if (hd != null && hd.ChiTietDonHangs.Any())
                {
                    // Thêm từng chi tiết đơn hàng
                    foreach (ChiTietDonHang a in hd.ChiTietDonHangs)
                    {
                        // Đảm bảo dữ liệu không bị null
                        if (a.MaSp == null || a.SoLuong <= 0)
                            continue;

                        ChiTietDonHang ct = new ChiTietDonHang
                        {
                            MaSp = a.MaSp,
                            SoLuong = a.SoLuong,
                            
                            iDDonHang = dhx.iDDonHang
                        };
                        dhx.ChiTietDonHangs.Add(ct);
                    }

                    try
                    {
                        // Thêm đơn hàng vào cơ sở dữ liệu
                        db.DonHangs.Add(dhx);
						foreach (ChiTietDonHang chiTiet in dhx.ChiTietDonHangs)
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
                        return RedirectToAction("SanPham");
                    }
                    catch (Exception ex)
                    {
                        // Log lỗi hoặc xử lý ngoại lệ
                        ModelState.AddModelError(string.Empty, "Lỗi khi lưu đơn hàng: " + ex.Message);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Đơn hàng rỗng, không thể lưu.");
                }
            }

            // Nếu xảy ra lỗi, quay lại giỏ hàng
            DonHang currentOrder = MySessions.Get<DonHang>(HttpContext.Session, "donhang");
            return View("FormGioHang", currentOrder);
        }

        
		
		public IActionResult xoaGioHang(string xoagh)
        {
            DonHang hd = MySessions.Get<DonHang>(HttpContext.Session, "donhang");
            if (hd != null)
            {
                foreach (ChiTietDonHang a in hd.ChiTietDonHangs.Where(t => t.MaSp == xoagh).ToList())
                {
                    hd.ChiTietDonHangs.Remove(a);

                }
                MySessions.Set<DonHang>(HttpContext.Session, "donhang", hd);
            }
            return RedirectToAction("FormGioHang");
        }
		public IActionResult TinTuc()
		{
			
			return View();
		}
		public IActionResult TinTuc2()
		{

			return View();
		}
		public IActionResult KhachHang()
		{

			return View();
		}
		public IActionResult CuaHang()
		{

			return View();
		}
		public IActionResult cuaHangTruong()
		{

			return View();
		}
		public IActionResult nhanVienBanHang()
		{

			return View();
		}
		public IActionResult sapXepSanPham(string sortOrder)
		{
			var sanPhams = db.SanPhams
					.Include(sp => sp.MaLoaiNavigation)
					.Include(sp => sp.MaLoaiNavigation.MaThNavigation)
					
					.AsQueryable();

			// Sắp xếp theo giá tiền
			if (sortOrder == "price_asc")
			{
				sanPhams = sanPhams.OrderBy(sp => sp.GiaBan);
			}
			else if (sortOrder == "price_desc")
			{
				sanPhams = sanPhams.OrderByDescending(sp => sp.GiaBan);
			}
			// Chuyển dữ liệu sang danh sách và trả về View
			return View(sanPhams.ToList());
		}
		public IActionResult HienThiSanPhamTheoSize(string size)
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
			// Chuyển đổi kích cỡ từ chuỗi sang số nguyên
			if (!int.TryParse(size, out int sizeInt))
			{
				return RedirectToAction("Index"); // Trả về danh sách mặc định nếu không chuyển đổi được
			}

			// Lọc sản phẩm theo kích cỡ
			var sanPhamTheoSize = db.SanPhams.Where(sp => sp.KichCo == sizeInt).ToList();
			return View(sanPhamTheoSize);
		}
		public IActionResult hienThiSanPhamTheoGia(string gia)
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
			if (gia == "Dưới 1.000.000đ")
			{
				var sanPhamTheoGia = db.SanPhams.Where(sp => sp.GiaBan < 1000000).ToList();
				return View(sanPhamTheoGia);
			}
			else if (gia == "1.000.000đ - 2.000.000đ")
			{
				var sanPhamTheoGia = db.SanPhams.Where(sp => sp.GiaBan >= 1000000 && sp.GiaBan < 2000000).ToList();
				return View(sanPhamTheoGia);
			}
			else if (gia == "2.000.000đ - 3.000.000đ")
			{
				var sanPhamTheoGia = db.SanPhams.Where(sp => sp.GiaBan >= 2000000 && sp.GiaBan < 3000000).ToList();
				return View(sanPhamTheoGia);
			}
			else if (gia == "3.000.000đ - 4.000.000đ")
			{
				var sanPhamTheoGia = db.SanPhams.Where(sp => sp.GiaBan >= 3000000 && sp.GiaBan < 4000000).ToList();
				return View(sanPhamTheoGia);
			}
			else
			{
				var sanPhamTheoGia = db.SanPhams.Where(sp => sp.GiaBan >= 4000000).ToList();
                return View(sanPhamTheoGia);
			}
			// Chuyển dữ liệu sang danh sách và trả về View

		}
		
		// Chuyển đổi kích cỡ từ chuỗi sang số nguyên
		public IActionResult hienThiSanPhamTheoLoaiVaThuongHieu(string tenLoai,string tenTH)
		{
			List<SanPham> dssp = db.SanPhams.ToList();

			// Cập nhật các thông tin liên quan đến các loại sản phẩm và thương hiệu
			foreach (SanPham item in dssp)
			{
				item.MaLoaiNavigation = db.LoaiSanPhams.Find(item.MaLoai);
				List<LoaiSanPham> dsloai = db.LoaiSanPhams.ToList();
				foreach (LoaiSanPham loai in dsloai)
				{
					loai.MaThNavigation = db.ThuongHieus.Find(loai.MaTh);
				}
			}
			if (!string.IsNullOrEmpty(tenLoai))
			{
				dssp = dssp.Where(sp => sp.MaLoaiNavigation.TenLoai == tenLoai).ToList();
			}
			if (!string.IsNullOrEmpty(tenTH))
			{
				dssp = dssp.Where(sp => sp.MaLoaiNavigation.MaThNavigation.TenTh == tenTH).ToList();
			}
			TempData["SuccessMessage"] = tenLoai;
			ViewBag.SuccessMessage = TempData["SuccessMessage"];
			TempData["SuccessMessage"] = tenTH;
			ViewBag.SuccessMessage1 = TempData["SuccessMessage"];

			// Kiểm tra nếu tenLoai không null hoặc rỗng, tiến hành lọc danh sách sản phẩm


			// Truyền danh sách sản phẩm vào View
			return View(dssp);
		}
		public IActionResult timKiem(string timk)
		{
			// Lấy danh sách sản phẩm từ cơ sở dữ liệu và include các bảng liên quan (LoaiSanPham và ThuongHieu)
			List<SanPham> dssp = db.SanPhams
									.Include(sp => sp.MaLoaiNavigation) // Include loại sản phẩm
									.ThenInclude(loai => loai.MaThNavigation) // Include thương hiệu
									.ToList();

			// Lọc sản phẩm theo từ khóa
			if (!string.IsNullOrEmpty(timk))
			{
				dssp = dssp.Where(sp =>
							   (sp.TenSp.Contains(timk, StringComparison.OrdinalIgnoreCase))
							   || (sp.MaLoaiNavigation != null && sp.MaLoaiNavigation.TenLoai.Contains(timk, StringComparison.OrdinalIgnoreCase))
							   || (sp.MaLoaiNavigation != null && sp.MaLoaiNavigation.MaThNavigation != null && sp.MaLoaiNavigation.MaThNavigation.TenTh.Contains(timk, StringComparison.OrdinalIgnoreCase))
						   )
						  .ToList();
			}

			// Cập nhật các thông tin liên quan đến các loại sản phẩm và thương hiệu
			foreach (SanPham item in dssp)
			{
				// Kiểm tra nếu MaLoaiNavigation là null
				if (item.MaLoaiNavigation == null)
				{
					item.MaLoaiNavigation = db.LoaiSanPhams.Find(item.MaLoai);
				}

				// Kiểm tra lại MaLoaiNavigation và MaThNavigation trước khi truy cập
				if (item.MaLoaiNavigation != null)
				{
					if (item.MaLoaiNavigation.MaThNavigation == null)
					{
						item.MaLoaiNavigation.MaThNavigation = new ThuongHieu();  // Tạo đối tượng mới nếu không có thương hiệu
					}
				}
			}

			TempData["SuccessMessage"] = timk;
			ViewBag.SuccessMessage = TempData["SuccessMessage"];
			// Truyền danh sách sản phẩm đã lọc sang view
			return View(dssp);
		}
		public IActionResult sanPhamMoi()
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
				ViewBag.SuccessMessagehetSP = TempData["SuccessMessagehetSP"];
				return View(db.SanPhams.ToList());
			
			
		}
		






	}















}

