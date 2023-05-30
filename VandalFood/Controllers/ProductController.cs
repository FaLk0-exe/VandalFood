using Microsoft.AspNetCore.Mvc;
using VandalFood.BLL.Services;

namespace VandalFood.Controllers
{

    public class ProductController:Controller
    {
        public ActionResult Get([FromServices] ProductService productService)
        {
            return View(productService.Get());
        }

        public ActionResult Details([FromServices] ProductService productService,int id)
        {
            var product = productService.Get(id);
            if (product is null)
                return NotFound();
            return View(product);
        }
    }
}
