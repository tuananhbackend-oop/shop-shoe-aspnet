using LVTNWEBGIAYDEP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LVTNWEBGIAYDEP.Controllers
{
	public class LienHeController : Controller
	{
        private readonly DBGiayDepContext db;

        public LienHeController(DBGiayDepContext context)
        {
            db = context;
        }
        public IActionResult LienHe()
        {
			List<LienHe> dslh = db.LienHes.ToList();
			foreach (LienHe l in dslh)
			{
				l.TaiKhoanNavigation = db.TaiKhoans.Find(l.TaiKhoan);
			}
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
         
            
            return View(dslh);
        }
       
        public IActionResult Index()
        {
			if (HttpContext.Session.GetString("UserEmail") != null)
			{
				var userEmail = HttpContext.Session.GetString("UserEmail");
				var user = db.TaiKhoans
					.Include(t => t.LienHes) // Nạp danh sách liên hệ
					.FirstOrDefault(x => x.TaiKhoan1 == userEmail);

				if (user != null)
				{
					// Tạo ViewModel để chứa thông tin cần thiết
					var model = new LienHe
					{
						TaiKhoanNavigation = user, // Gán TaiKhoan vào liên hệ
					};
                    ViewBag.SuccessMessage = TempData["SuccessMessage"];
                    ViewBag.ErrorMessage = TempData["ErrorMessage"];
                    ViewBag.ErrorMessagelh = TempData["ErrorMessagelh"];
                    return View(model); // Truyền model này vào View
				}
			}

			return RedirectToAction("DangNhap", "DangNhap");
		}
		public IActionResult themLienHe(LienHe lh)
		{
			// Kiểm tra nếu model là null
			var isTaiKhoanHopLe = db.TaiKhoans.Any(t => t.TaiKhoan1 == lh.TaiKhoan);
			if (!isTaiKhoanHopLe)
			{
				// Thêm lỗi vào ModelState
				TempData["SuccessMessage"] = "Tài khoản không tồn tại!";

				return RedirectToAction("Index");
			}



			db.LienHes.Add(lh);
			lh.trangThai = "Chưa xem";
				db.SaveChanges();
            TempData["SuccessMessage"] = "Đã gửi liên hệ đến Admin!";
            return RedirectToAction("Index");
		
		}
        public IActionResult xoaLienHe(string taiKhoan)
        {
            // Lấy tất cả các liên hệ có tài khoản giống với tài khoản gửi về
            var lienHes = db.LienHes.Where(l => l.TaiKhoan == taiKhoan).ToList();

            // Xoá tất cả các liên hệ tìm được
            db.LienHes.RemoveRange(lienHes);

            // Lưu thay đổi vào cơ sở dữ liệu
            db.SaveChanges();

            // Thông báo thành công
            TempData["SuccessMessage"] = "Xoá liên hệ thành công !";

            // Chuyển hướng lại trang danh sách liên hệ
            return RedirectToAction("LienHe");
        }

        public IActionResult formPhanhoi(string taiKhoan)
        {
            // Lấy danh sách các liên hệ của người dùng
            List<LienHe> lienhList = db.LienHes.Where(l => l.TaiKhoan == taiKhoan).ToList();

            // Cập nhật trạng thái cho các liên hệ của người dùng
            foreach (var lienHe in lienhList)
            {
                lienHe.trangThai = "Đã xem";  // Cập nhật trạng thái
            }

            // Lưu thay đổi vào cơ sở dữ liệu
            db.SaveChanges();

            // Trả về View với danh sách liên hệ
            return View(lienhList);
        }

        [HttpPost]
        public IActionResult ThemPhanHoi(string taiKhoan, string noiDungPhanHoi)
        {
            // Find the LienHe record for the user (TaiKhoan)
            var lienHe = new LienHe
            {
                TaiKhoan = taiKhoan,
                // This can be captured from a user input form (if it's separate)
                noiDungPhanHoi = noiDungPhanHoi,
                // Add other necessary properties
              
            };

            // Add the reply to the database
            db.LienHes.Add(lienHe);
            db.SaveChanges();

            // Redirect back to show the updated conversation
            return RedirectToAction("formPhanhoi", new { taiKhoan = taiKhoan });
        }
        public IActionResult xoaPhanHoi(int phoi, string tkhoan)
        {
            // Find the LienHe record for the user (TaiKhoan)
          LienHe l = db.LienHes.Where(t=>t.Id==phoi).FirstOrDefault();

            // Add the reply to the database
            db.Remove(l);
            db.SaveChanges();

            // Redirect back to show the updated conversation
            return RedirectToAction("formPhanhoi", new { taiKhoan = tkhoan });
        }
        public IActionResult suaPhanHoi(int Id, string tkhoan, string ndphnew)
        {
            var l = db.LienHes.FirstOrDefault(t => t.Id == Id);

            if (l == null)
            {
                TempData["ErrorMessage"] = "Bản ghi không tồn tại!";
                return RedirectToAction("formPhanhoi", new { taiKhoan = tkhoan });
            }

            Console.WriteLine("Bản ghi đã tìm thấy với Id: " + Id);  // Kiểm tra giá trị Id
            l.noiDungPhanHoi = ndphnew;

            try
            {
                db.Update(l);
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra trong khi cập nhật dữ liệu. Vui lòng thử lại!";
                return RedirectToAction("formPhanhoi", new { taiKhoan = tkhoan });
            }

            return RedirectToAction("formPhanhoi", new { taiKhoan = l.TaiKhoan });
        }
        public IActionResult formNhanTin(string taiKhoan)
        {
            // Lấy danh sách các liên hệ của người dùng
            List<LienHe> lienhList = db.LienHes.Where(l => l.TaiKhoan == taiKhoan).ToList();

            if (lienhList == null || lienhList.Count == 0)  // Kiểm tra nếu danh sách trống hoặc null
            {
                TempData["ErrorMessagelh"] = "Bạn chưa gửi liên hệ, không thể kết nối!";
                return RedirectToAction("Index");
            }

            // Cập nhật trạng thái cho các liên hệ của người dùng
            foreach (var lienHe in lienhList)
            {
                lienHe.trangThai = "Chưa xem";  // Cập nhật trạng thái
            }

            // Lưu thay đổi vào cơ sở dữ liệu
            db.SaveChanges();

            // Trả về View với danh sách liên hệ
            return View(lienhList);
        }
        public IActionResult ThemNoiDung(string taiKhoan, string noiDungPhanHoi)
        {
            // Find the LienHe record for the user (TaiKhoan)
            var lienHe = new LienHe
            {
                TaiKhoan = taiKhoan,
                // This can be captured from a user input form (if it's separate)
                NoiDung = noiDungPhanHoi,
                // Add other necessary properties

            };

            // Add the reply to the database
            db.LienHes.Add(lienHe);
            db.SaveChanges();

            // Redirect back to show the updated conversation
            return RedirectToAction("formNhanTin", new { taiKhoan = taiKhoan });
        }
        public IActionResult xoaNoiDung(int phoi, string tkhoan)
        {
            // Find the LienHe record for the user (TaiKhoan)
            LienHe l = db.LienHes.Where(t => t.Id == phoi).FirstOrDefault();

            // Add the reply to the database
            db.Remove(l);
            db.SaveChanges();

            // Redirect back to show the updated conversation
            return RedirectToAction("formNhanTin", new { taiKhoan = tkhoan });
        }
        public IActionResult suaNoiDung(int Id, string tkhoan, string ndphnew)
        {
            var l = db.LienHes.FirstOrDefault(t => t.Id == Id);

            if (l == null)
            {
                TempData["ErrorMessage"] = "Bản ghi không tồn tại!";
                return RedirectToAction("formNhanTin", new { taiKhoan = tkhoan });
            }

            Console.WriteLine("Bản ghi đã tìm thấy với Id: " + Id);  // Kiểm tra giá trị Id
            l.NoiDung = ndphnew;

            try
            {
                db.Update(l);
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra trong khi cập nhật dữ liệu. Vui lòng thử lại!";
                return RedirectToAction("formNhanTin", new { taiKhoan = tkhoan });
            }

            return RedirectToAction("formNhanTin", new { taiKhoan = l.TaiKhoan });
        }
        public IActionResult xoaLienHeUser(string taiKhoan)
        {
            // Lấy tất cả các liên hệ có tài khoản giống với tài khoản gửi về
            var lienHes = db.LienHes.Where(l => l.TaiKhoan == taiKhoan).ToList();

            // Xoá tất cả các liên hệ tìm được
            db.LienHes.RemoveRange(lienHes);

            // Lưu thay đổi vào cơ sở dữ liệu
            db.SaveChanges();

            // Thông báo thành công
            TempData["SuccessMessage"] = "Xoá liên hệ thành công !";

            // Chuyển hướng lại trang danh sách liên hệ
            return RedirectToAction("Index");
        }
        

    }
}
