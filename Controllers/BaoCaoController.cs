using LVTNWEBGIAYDEP.Models;
using Microsoft.AspNetCore.Mvc;

namespace LVTNWEBGIAYDEP.Controllers
{

    public class BaoCaoController : Controller
    {

        private readonly DBGiayDepContext db;

        public BaoCaoController(DBGiayDepContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult baoCao()
        {
         
            return View();
        }
        public IActionResult sanPhamBanChay()
        {
            return View();
        }
        public IActionResult bieuDo()
        {
            return View();
        }
        public IActionResult bieuDonam()
        {
            return View();
        }

       
      
   
    }
}
