using EFCORE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace EFCORE.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _db;
        public HomeController(ApplicationDbContext db)

        {
            _db = db;
        }
        public IActionResult Index()
        {
            var pageSize = 4;
            var dsSanPham = _db.Products.Include(x => x.Category).ToList();
            return View(dsSanPham.Skip(0).Take(pageSize).ToList());
        }
        public IActionResult LoadMore(int page = 1)
        {
            var pageSize = 4;
            var dsSanPham = _db.Products.Include(x => x.Category).ToList();
            return PartialView("_ProductPartial", dsSanPham.Skip((page - 1) * pageSize).Take(pageSize).ToList());
        }
        public IActionResult Privacy()
        {
            return View();
        }
    }
}
