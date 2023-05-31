using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using VandalFood.BLL.Services;
using VandalFood.DAL.Models;
using Hosting = Microsoft.AspNetCore.Hosting;

namespace VandalFood.Controllers
{

    public class ProductController : Controller
    {
        private Hosting.IHostingEnvironment _env;
        public ProductController(Hosting.IHostingEnvironment env)
        {
            _env = env;
        }
        public ActionResult Get([FromServices] ProductService productService)
        {
            var products = productService.Get();
            if (!HttpContext.User.IsInRole("Admin"))
                products = products.Where(s => s.IsActive);
            return View(products);
        }

        public ActionResult Details([FromServices] ProductService productService, int id)
        {
            var product = productService.Get(id);
            if (product is null)
                return NotFound();

            if (!product.IsActive)
            {
                if (HttpContext.User.IsInRole("Admin"))
                {
                    return View(product);
                }
                else
                {
                    return NotFound();
                }
            }
            return View(product);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Toggle([FromServices] ProductService productService, int id)
        {
            var product = productService.Get(id);
            if (product is null)
                return NotFound();
            product.IsActive = !product.IsActive;
            productService.Update(product);
            return RedirectToAction(actionName: "Get");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit([FromServices] ProductService productService, string? message, int id)
        {
            if (message is not null)
            {
                ViewData["message"] = message;
            }
            var product = productService.Get(id);
            if (product is null)
                return NotFound();
            return View(product);
        }

        public ActionResult TryEdit([FromServices] ProductService productService, Product product, IFormFile photo)
        {
            if (photo != null)
            {
                try
                {
                    System.IO.File.Delete(Path.Combine(_env.WebRootPath, "images", product.PhotoPath));
                }
                catch { }
                string uniqueImageName = Guid.NewGuid().ToString()+ Path.GetExtension(photo.FileName);
                var imagePath = Path.Combine(_env.WebRootPath,"images", uniqueImageName);
                using (var fileStream = new FileStream(imagePath, FileMode.Create, FileAccess.ReadWrite))
                {
                    photo.CopyTo(fileStream);
                }
                product.PhotoPath = uniqueImageName;
            }
            try
            {
                productService.Update(product);
            }
            catch (Exception ex)
            {
                return RedirectToAction(actionName: "Edit", routeValues: new { id = product.Id, message = ex.Message });
            }
            return RedirectToAction(actionName: "Get");
        }

        public ActionResult Create(string? message)
        {
            if (message is not null)
            {
                ViewData["message"] = message;
            }
            return View();
        }

        public ActionResult TryCreate([FromServices] ProductService productService, Product product, IFormFile photo)
        {
            try
            {
                if (photo != null)
                {
                    string uniqueImageName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
                    var imagePath = Path.Combine(_env.WebRootPath, "images", uniqueImageName);
                    using (var fileStream = new FileStream(imagePath, FileMode.Create, FileAccess.ReadWrite))
                    {
                        photo.CopyTo(fileStream);
                    }
                    product.PhotoPath = uniqueImageName;
                }
                productService.Create(product);
                return RedirectToAction(actionName: "Get");
            }
            catch (Exception ex)
            {
                return RedirectToAction(actionName: "Create", routeValues: new { message = ex.Message });
            }
        }
    }
}
