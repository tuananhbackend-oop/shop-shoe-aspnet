using LVTNWEBGIAYDEP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LVTNWEBGIAYDEP.Controllers
{
	public class QLThuongHieuController : Controller
	{
        private readonly DBGiayDepContext db;

        public QLThuongHieuController(DBGiayDepContext context)
        {
            db = context;
        }
        public IActionResult Index()
		{
			return View();
		}
        public IActionResult TenSP(string tenTH)
        {
            var dssp = db.SanPhams
                .Include(sp => sp.MaLoaiNavigation)
                .ThenInclude(loai => loai.MaThNavigation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(tenTH))
            {
                dssp = dssp.Where(sp => sp.MaLoaiNavigation.MaThNavigation.TenTh == tenTH);
            }
            ViewBag.Brand = tenTH;
            return View(dssp.ToList());
        }

    }
}
