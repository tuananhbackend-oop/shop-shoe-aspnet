using LVTNWEBGIAYDEP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace LVTNWEBGIAYDEP.Controllers
{
    public class DangNhapController : Controller
    {
        private readonly DBGiayDepContext db;

        public DangNhapController(DBGiayDepContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UserEmail") != null)
            {
                // Retrieve the logged-in user's email
                var userEmail = HttpContext.Session.GetString("UserEmail");
                var user = db.TaiKhoans.FirstOrDefault(x => x.TaiKhoan1 == userEmail);

                if (user != null)
                {
                    if (user.trangThai == "Hoạt Động")
                    {

                        // Kiểm tra tài khoản admin trước
                        if (user.phanQuyen == "Admin")
                        {
                      
                            return RedirectToAction("Index", "Admin");
                        }
                        else if (user.phanQuyen == "User")
                        {
                            var ds = db.DonHangs.Where(d => d.TaiKhoan == userEmail).ToList();

                            // Pass the user and order list to the view
                            ViewData["User"] = user;
                            ViewData["Orders"] = ds;
                            // Pass the user object to the view
                            return View(user);
                          
                        }
                        else if (user.phanQuyen == "Quản trị viên")
                        {
                          
                            return RedirectToAction("Index", "QuanTriVien");
                        }
                        else if (user.phanQuyen == "Nhân viên bán hàng")
                        {
                        
                            return RedirectToAction("Index", "NhanVienBanHang");
                        }
                        else if (user.phanQuyen == "Nhân viên quản lý sản phẩm")
                        {
                           
                            return RedirectToAction("Index", "NhanVienQLSP");
                        }
                        else if (user.phanQuyen == "Nhân viên chăm sóc khách hàng")
                        {
                           
                            return RedirectToAction("Index", "NhanVienCSKH");
                        }
                        TempData["SuccessMessage"] = "Đăng nhập thành công!";
                    }
                 // This will pass the user to the view as the model
                }
            }

            // If the user is not logged in, redirect to login page
            return View("DangNhap");
        

			// Nếu chưa đăng nhập, hiển thị form đăng nhập
		
		}
		public IActionResult xoaDonHang(int idhd, string LyDo)
		{
			using (var transaction = db.Database.BeginTransaction())
			{
				try
				{
					// Lấy các chi tiết đơn hàng theo ID đơn hàng
					var chiTietDonHangs = db.ChiTietDonHangs.Where(ctdh => ctdh.iDDonHang == idhd).ToList();


					// Hoàn trả số lượng vào hàng tồn kho
					foreach (var chiTiet in chiTietDonHangs)
					{
						var sanPham = db.SanPhams.FirstOrDefault(sp => sp.MaSp == chiTiet.MaSp);
						if (sanPham != null)
						{
							sanPham.HangTonKho += chiTiet.SoLuong; // Hoàn trả số lượng
						}
					}

					// Lưu thay đổi vào cơ sở dữ liệu (hàng tồn kho)
					db.SaveChanges();

					// Xóa các chi tiết đơn hàng


					// Xóa đơn hàng
					var dh = db.DonHangs.Find(idhd);
					if (dh != null)
					{
						dh.trangThai = "Đã Huỷ";
						dh.noiDungHuy = LyDo;
					}
					db.SaveChanges();

					// Xác nhận giao dịch
					transaction.Commit();

					
				}
				catch (Exception ex)
				{
					// Rollback nếu có lỗi
					transaction.Rollback();
					TempData["ErrorMessage"] = "Không thể xóa đơn hàng: " + ex.Message;
				}
			}

			// Quay lại trang danh sách hóa đơn
			return RedirectToAction("Index");
		}
		public IActionResult DangKi()
		{
			ViewBag.SuccessMessageThieu = TempData["SuccessMessageThieu"];
			return View();
		}
		public IActionResult themTaiKhoan(TaiKhoan tk)
		{
		var tkhon = db.TaiKhoans.Where(a=>a.TaiKhoan1 == tk.TaiKhoan1);
		if (tkhon.Count() == 0) 
		{
			if (ModelState.IsValid)
			{
					tk.MatKhau = BCrypt.Net.BCrypt.HashPassword(tk.MatKhau);
					tk.trangThai = "Hoạt Động";
					tk.phanQuyen = "User";

					db.TaiKhoans.Add(tk);

					db.SaveChanges();


					// Gửi thông báo thành công vào TempData
					TempData["SuccessMessagedk"] = "Đăng ký thành công!";
					return RedirectToAction("DangNhap");

				}
		}
		else
		{
				TempData["SuccessMessageThieu"] = "Tài khyoản đả tồn tại !";
				return RedirectToAction("DangKi");
		}	

			TempData["SuccessMessageThieu"] = "Vui lòng nhập đầy đủ thông tin !";
		
			return RedirectToAction("DangKi"); // Chuyển hướng đến trang đăng nhập
		}

		public IActionResult DangNhap()
		{
			
			ViewBag.SuccessMessagedk = TempData["SuccessMessagedk"];
			// Truyền SuccessMessage từ TempData sang ViewBag
			ViewBag.SuccessMessage5 = TempData["SuccessMessage5"];
			ViewBag.SuccessMessagedb = TempData["SuccessMessagedb"];
			ViewBag.SuccessMessage = TempData["SuccessMessage"];

            return View();
		}
		public IActionResult qlDangNhap(string customerEmail, string customerPassword)
        {
			TaiKhoan tkhoan = db.TaiKhoans.Find(customerEmail);
			
			if (tkhoan != null )
			{
				if (BCrypt.Net.BCrypt.Verify(customerPassword, tkhoan.MatKhau))
				{
					if (tkhoan.trangThai == "Hoạt Động")
					{

						// Kiểm tra tài khoản admin trước
						if (tkhoan.phanQuyen == "Admin")
						{
							HttpContext.Session.SetString("UserEmail", customerEmail);
							return RedirectToAction("Index", "Admin");
						}
						else if (tkhoan.phanQuyen == "User")
						{
							HttpContext.Session.SetString("UserEmail", customerEmail);
							TempData["SuccessMessage"] = "Đăng nhập thành công!";
							return RedirectToAction("Index", "User");
						}
						else if (tkhoan.phanQuyen == "Quản trị viên")
						{
							HttpContext.Session.SetString("UserEmail", customerEmail);
							return RedirectToAction("Index", "QuanTriVien");
						}
						else if (tkhoan.phanQuyen == "Nhân viên bán hàng")
						{
							HttpContext.Session.SetString("UserEmail", customerEmail);
							return RedirectToAction("Index", "NhanVienBanHang");
						}
						else if (tkhoan.phanQuyen == "Nhân viên quản lý sản phẩm")
						{
							HttpContext.Session.SetString("UserEmail", customerEmail);
							return RedirectToAction("Index", "NhanVienQLSP");
						}
						else if (tkhoan.phanQuyen == "Nhân viên chăm sóc khách hàng")
						{
							HttpContext.Session.SetString("UserEmail", customerEmail);
							return RedirectToAction("Index", "NhanVienCSKH");
						}

					}
					else
					{
						TempData["SuccessMessagedb"] = "Tài khoản đã bị đóng băng";
						return RedirectToAction("DangNhap");
					}
				}
				TempData["SuccessMessage5"] = "Email hoặc mật khẩu không chính xác";
				return RedirectToAction("DangNhap");

			}
			TempData["SuccessMessage5"] = "Email hoặc mật khẩu không chính xác";
			return RedirectToAction("DangNhap");

			// Nếu không khớp tài khoản nào, trả về lỗi

		}
        public IActionResult Logout()
        {
            // Đăng xuất, xóa thông tin session
            HttpContext.Session.Clear(); // 
            return RedirectToAction("Index","User");
        }
	

		public IActionResult hoanTat(int idhd)
		{
			try
			{
				// Tìm kiếm các chi tiết đơn hàng theo ID đơn hàng
				var chiTietDonHangs = db.ChiTietDonHangs.Where(ctdh => ctdh.iDDonHang == idhd).ToList();

				// Xóa các chi tiết đơn hàng
			

				// Tìm kiếm và xóa đơn hàng

				DonHang dh = db.DonHangs.Find(idhd);
				if (dh != null)
				{
					dh.trangThai = "Hoàn Tất";
				}
				db.SaveChanges();
			
			}
			catch (Exception ex)
			{
				TempData["ErrorMessage"] = "lỗi";
			}

			// Quay lại trang danh sách hóa đơn
			return RedirectToAction("Index");
		}
		public IActionResult khongNhanHang(int idhd, string LyDo)
		{
			using (var transaction = db.Database.BeginTransaction())
			{
				try
				{
					// Lấy các chi tiết đơn hàng theo ID đơn hàng
					var chiTietDonHangs = db.ChiTietDonHangs.Where(ctdh => ctdh.iDDonHang == idhd).ToList();
					var dh = db.DonHangs.Find(idhd);
					if (LyDo == "Lỗi sản phẩm")
					{

						if (dh != null)
						{
							dh.trangThai = "Không Nhận";
							dh.noiDungHuy = LyDo;
						}
						db.SaveChanges();
						transaction.Commit();
						// Hoàn trả số lượng vào hàng tồn kho

					}
					else
					{
						foreach (var chiTiet in chiTietDonHangs)
						{
							var sanPham = db.SanPhams.FirstOrDefault(sp => sp.MaSp == chiTiet.MaSp);
							if (sanPham != null)
							{
								sanPham.HangTonKho += chiTiet.SoLuong; // Hoàn trả số lượng
							}
						}
						// Lưu thay đổi vào cơ sở dữ liệu (hàng tồn kho)
						db.SaveChanges();
						if (dh != null)
						{
							dh.trangThai = "Không Nhận";
							dh.noiDungHuy = LyDo;
						}
						db.SaveChanges();

						// Xác nhận giao dịch
						transaction.Commit();
					}

					
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





	}
}
