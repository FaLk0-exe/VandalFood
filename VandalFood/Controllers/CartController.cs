using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using VandalFood.DAL.Models;
using VandalFood.DAL.Repositories;
using VandalFood.Models.Cart;

namespace VandalFood.Controllers
{
    public class CartController : Controller
    {
        public CartController()
        {

        }

        public ActionResult Details()
        {
            var cartJson = HttpContext.Session.GetString("cart");
            List<CartModel> cart;
            if(cartJson is not null)
                cart = JsonSerializer.Deserialize<List<CartModel>>(cartJson);
            else
                cart = new List<CartModel>();
            return View(cart);
        }

        public ActionResult Add([FromServices] ProductRepository productRepository, int id)
        {
            var product = productRepository.Get(id);
            if (product is null)
                return NotFound();
            AddToCart(product, id);
            return RedirectToAction(controllerName: "product", actionName: "get");
        }

        public ActionResult AddQuick([FromServices] ProductRepository productRepository, int id)
        {
            var product = productRepository.Get(id);
            if (product is null)
                return NotFound();
            AddToCart(product, id);
            return RedirectToAction(controllerName: "cart", actionName: "details");
        }

        public ActionResult Increase([FromServices] ProductRepository productRepository, int id)
        {
            var product = productRepository.Get(id);
            if (product is null)
                return NotFound();
            List<CartModel> cartProducts;
            var cartRaw = HttpContext.Session.GetString("cart");
            cartProducts = JsonSerializer.Deserialize<List<CartModel>>(cartRaw);
            var existedItem = cartProducts.FirstOrDefault(s => s.Product.Id == product.Id);
            existedItem.Count++;
            HttpContext.Session.SetString("cart", JsonSerializer.Serialize(cartProducts));
            HttpContext.Session.SetInt32("cartCount", cartProducts.Count());
            return RedirectToAction(controllerName: "cart", actionName: "details");
        }

        public ActionResult Decrease([FromServices] ProductRepository productRepository, int id)
        {
            var product = productRepository.Get(id);
            if (product is null)
                return NotFound();
            List<CartModel> cartProducts;
            var cartRaw = HttpContext.Session.GetString("cart");
            cartProducts = JsonSerializer.Deserialize<List<CartModel>>(cartRaw);
            var existedItem = cartProducts.FirstOrDefault(s => s.Product.Id == product.Id);
            if (existedItem.Count != 1)
            {
                existedItem.Count--;
            }
            HttpContext.Session.SetString("cart", JsonSerializer.Serialize(cartProducts));
            HttpContext.Session.SetInt32("cartCount", cartProducts.Count());
            return RedirectToAction(controllerName: "cart", actionName: "details");
        }

        public ActionResult Remove([FromServices] ProductRepository productRepository, int id)
        {
            var product = productRepository.Get(id);
            if (product is null)
                return NotFound();
            List<CartModel> cartProducts;
            var cartRaw = HttpContext.Session.GetString("cart");
            cartProducts = JsonSerializer.Deserialize<List<CartModel>>(cartRaw);
            var existedItem = cartProducts.FirstOrDefault(s => s.Product.Id == product.Id);
            cartProducts.Remove(existedItem);
            HttpContext.Session.SetString("cart", JsonSerializer.Serialize(cartProducts));
            HttpContext.Session.SetInt32("cartCount", cartProducts.Count());
            return RedirectToAction(controllerName: "cart", actionName: "details");
        }

        public ActionResult AddMulti([FromServices] ProductRepository productRepository, int id, int quantity)
        {
            var product = productRepository.Get(id);
            if (product is null)
                return NotFound();
            List<CartModel> cartProducts;
            var cartRaw = HttpContext.Session.GetString("cart");
            if (cartRaw is null)
            {
                cartProducts = new List<CartModel>();
                cartProducts.Add(new CartModel { Product = product, Count = quantity });
            }
            else
            {
                cartProducts = JsonSerializer.Deserialize<List<CartModel>>(cartRaw);
                var existedItem = cartProducts.FirstOrDefault(s => s.Product.Id == product.Id);
                if (existedItem is null)
                {
                    existedItem = new CartModel
                    {
                        Product = product,
                        Count = quantity
                    };
                    cartProducts.Add(existedItem);
                }
                else
                {
                    existedItem.Count += quantity;
                }
            }
            HttpContext.Session.SetString("cart", JsonSerializer.Serialize(cartProducts));
            HttpContext.Session.SetInt32("cartCount", cartProducts.Count());
            return RedirectToAction(controllerName: "product", actionName: "details", routeValues: new { id = product.Id });
        }

        private void AddToCart(Product product, int id)
        {
            
            List<CartModel> cartProducts;
            var cartRaw = HttpContext.Session.GetString("cart");
            if (cartRaw is null)
            {
                cartProducts = new List<CartModel>();
                cartProducts.Add(new CartModel { Product = product, Count = 1 });
            }
            else
            {
                cartProducts = JsonSerializer.Deserialize<List<CartModel>>(cartRaw);
                var existedItem = cartProducts.FirstOrDefault(s => s.Product.Id == product.Id);
                if (existedItem is null)
                {
                    existedItem = new CartModel
                    {
                        Product = product,
                        Count = 1
                    };
                    cartProducts.Add(existedItem);
                }
                else
                {
                    existedItem.Count++;
                }
            }
            HttpContext.Session.SetString("cart", JsonSerializer.Serialize(cartProducts));
            HttpContext.Session.SetInt32("cartCount", cartProducts.Count());
        }
    }
}
