﻿using EFCORE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EFCORE.Controllers
{
    [Authorize(Roles =SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hosting;
        public ProductController(ApplicationDbContext db, IWebHostEnvironment hosting)
        {
            _db = db;
            _hosting = hosting;
        }
        #region View_Product
        //Hiển thị danh sách sản phẩm
        public IActionResult Index(int page = 1, int pageSize = 4)
        {
            var query = _db.Products.Include(x => x.Category).OrderBy(p => p.Id);

            int totalItems = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var products = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            ViewData["BootstrapVersion"] = "v1";

            return View(products);
        }

        #endregion
        //-----------------------------------------------------
        #region Add_Product
        //Hiển thị form thêm sản phẩm mới
        [HttpGet]
        public IActionResult Add()
        {
            //truyền danh sách thể loại cho View để sinh ra điều khiển DropDownList
            ViewBag.CategoryList = _db.Categories.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });
            return View();
        }
        //Xử lý thêm sản phẩm
        [HttpPost]
        public IActionResult Add(Product product, IFormFile ImageUrl)
        {
                if (ImageUrl != null)
                {
                    //xu ly upload và lưu ảnh đại diện
                    product.ImageUrl = SaveImage(ImageUrl);
                }
                //thêm product vào table Product
                _db.Products.Add(product);
                _db.SaveChanges();
                TempData["success"] = "Thêm sản phẩm thành công!";
                return RedirectToAction("Index");
        }
        #endregion
        //-----------------------------------------------------
        #region Update_Product
        //Hiển thị form cập nhật sản phẩm
        public IActionResult Update(int id)
        {
            var sp = _db.Products.Find(id);
            //truyền danh sách thể loại cho View để sinh ra điều khiển DropDownList
            ViewBag.CategoryList = _db.Categories.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });
            return View(sp);
        }
        [HttpPost]
        public IActionResult Update(Product product, IFormFile ImageUrl)
        {
            var OldProduct = _db.Products.Find(product.Id);
            if (ImageUrl != null)
            {
                //xử lý upload và lưu ảnh đại diện mới
                product.ImageUrl = SaveImage(ImageUrl);
                //xóa ảnh cũ (nếu có)
                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    var oldFilePath = Path.Combine(_hosting.WebRootPath, product.ImageUrl);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }
            }
            else
            {
                product.ImageUrl = OldProduct.ImageUrl;
                // product.ImageUrl=SaveImage(ImageUrl);
            }
            //cập nhật product vào table Product
            OldProduct.Name = product.Name;
            OldProduct.Description = product.Description;
            OldProduct.Price = product.Price;
            OldProduct.CategoryId = product.CategoryId;
            OldProduct.ImageUrl = product.ImageUrl;
            _db.SaveChanges();
            TempData["success"] = "Cập nhật sản phẩm thành công";
            return RedirectToAction("Index");

        }
        #endregion



        private string SaveImage(IFormFile image)
        {
            //đặt lại tên file cần lưu
            var filename = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            //lay duong dan luu tru wwwroot tren server
            var path = Path.Combine(_hosting.WebRootPath, @"images/products");
            var saveFile = Path.Combine(path, filename);
            using (var filestream = new FileStream(saveFile, FileMode.Create))
            {
                image.CopyTo(filestream);
            }
            return @"images/products/" + filename;
        }
        #region Delete_Product
        //Hiển thị form xác nhận xóa sản phẩm
        public IActionResult Delete(int id)
        {
            var product = _db.Products.Find(id);
           
            return View(product);
        }

        //Xử lý xóa sản phẩm
        
        public IActionResult DeleteConfirmed(int id)
        {
            var product = _db.Products.Find(id);
            // xoá hình cũ của sản phẩm
            if (!String.IsNullOrEmpty(product.ImageUrl))
            {
                var oldFilePath = Path.Combine(_hosting.WebRootPath, product.ImageUrl);
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
            }
            // xoa san pham khoi CSDL
            _db.Products.Remove(product);
            _db.SaveChanges();
            TempData["success"] = "Xóa Sản Phẩm thành công";
            //chuyen den action index
            return RedirectToAction("Index");
        }
        #endregion
    }
}
