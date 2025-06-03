using EFCORE.Areas.Customer.Models;
using EFCORE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCORE.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ProductController : Controller
    {

        private readonly ApplicationDbContext _db;

        public ProductController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index(int catid = 1)
        {
            var dsLoai = _db.Categories.OrderBy(x => x.DisplayOrder)
                .Select(c=> new CategoryViewModel { Id=c.Id,Name=c.Name,
                                                    TotalProduct=_db.Products.Where(p=>p.CategoryId==c.Id).Count()})
                .ToList();
            var dsSanPham = _db.Products
    .Include(p => p.Category) // 👈b
    .Where(p => p.CategoryId == catid)
    .ToList();
            var catName = _db.Categories.Find(catid).Name;
            //truyen qua view
            ViewBag.DSLOAI = dsLoai;
            ViewBag.CategoryName = catName;
            return View(dsSanPham);
        }

        //tra ve view khong dung layout
        public IActionResult LoadProduct(int catid = 1)
        {
            //lsy tu csdl
            var dsSanPham = _db.Products
        .Include(p => p.Category) // 👈b
        .Where(p => p.CategoryId == catid)
        .ToList();
            var catName = _db.Categories.Find(catid).Name;
            //truyen qua view
            ViewBag.CategoryName = catName;
            return PartialView("ProductPartial",dsSanPham);
        }
    }
}
