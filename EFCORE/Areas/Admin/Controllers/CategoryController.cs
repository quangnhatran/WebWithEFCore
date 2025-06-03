using EFCORE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EFCORE.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController: Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
        
_db = db;
        }
        //Hiển thị danh sách chủng loại
        public IActionResult Index(int page = 1)
        {
            int pageSize = 3; // Số item mỗi trang
            var totalItems = _db.Categories.Count(); // Tổng số Category
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize); // Tổng số trang

            var listCategory = _db.Categories
                                  .OrderBy(c => c.Id)
                                  .Skip((page - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewData["BootstrapVersion"] = "v2";

            return View(listCategory);
        }
        //Hiển thị form thêm mới chủng loại
        public IActionResult Add()
        {
            return View();
        }
        // Xử lý thêm chủng loại mới
        [HttpPost]
        public IActionResult Add(Category category)
        {
            if (ModelState.IsValid) //kiem tra hop le
            {
                //thêm category vào table Categories
                _db.Categories.Add(category);
                _db.SaveChanges();
                TempData["success"] = "Category Thêm Thành Công";
                return RedirectToAction("Index");
            }
            return View();
        }
        //Hiển thị form cập nhật chủng loại
        public IActionResult Update(int id)
        {
            var category = _db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        // Xử lý cập nhật chủng loại
        [HttpPost]
        public IActionResult Update(Category category)
        {
            if (ModelState.IsValid) //kiem tra hop le
            {
                //cập nhật category vào table Categories
                _db.Categories.Update(category);
                _db.SaveChanges();
                TempData["success"] = "Category updated success";
                return RedirectToAction("Index");
            }
            return View();
        }

        //Hiển thị form xác nhận xóa chủng loại
        public IActionResult Delete(int id)
        {
            var category = _db.Categories.FirstOrDefault(x => x.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        // Xử lý xóa chủng loại
        public IActionResult DeleteConfirmed(int id)
        {
            var category = _db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            _db.Categories.Remove(category);
            _db.SaveChanges();
            TempData["success"] = "Category deleted success";
            return RedirectToAction("Index");
        }
    }
}
