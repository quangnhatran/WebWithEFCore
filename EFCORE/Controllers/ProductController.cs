using EFCORE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCORE.Controllers
{
    public class ProductController:Controller
    {
        private readonly ApplicationDbContext _db;
        public ProductController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index(int catid = 1)
        {
            var dsSanPham = _db.Products.Include(x => x.Category).Where(x => x.CategoryId == catid).ToList();
            return View(dsSanPham);
        }
        public IActionResult GetCategory()
        {
            var dsTheLoai = _db.Categories.ToList();
            return PartialView("CategoryPartial", dsTheLoai);
        }
    }
}
