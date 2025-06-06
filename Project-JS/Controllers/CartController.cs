using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project_JS.Data;
using Project_JS.Models;
using System.Linq;

namespace Project_JS.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Cart()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetBooksByIds([FromBody] List<int> ids)
        {
            var books = _context.Books.Where(b => ids.Contains(b.Id)).ToList();
            return Json(books);
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var model = new OrderFormViewModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Checkout(OrderFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            
            var cart = GetCartFromLocalStorage();

            if (!cart.Any())
            {
                TempData["Error"] = "Корзина пуста.";
                return RedirectToAction("Cart");
            }

            var order = new Order
            {
                UserId = (int)userId,
                OrderDate = DateTime.Now,
                Status = "Pending", 
                FullName = model.FullName,
                Address = model.Address,
                PhoneNumber = model.PhoneNumber,
                AdditionalNotes = model.AdditionalNotes
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            
            foreach (var item in cart)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                };
                _context.OrderItems.Add(orderItem);
            }

            _context.SaveChanges();

            
            ClearCart();

            TempData["Success"] = "Ваше замовлення успішно оформлено!";
            return RedirectToAction("OrderSuccess");
        }

        public IActionResult OrderSuccess()
        {
            return View();
        }

        private List<CartItem> GetCartFromLocalStorage()
        {
            var cartJson = HttpContext.Session.GetString("cart");
            return string.IsNullOrEmpty(cartJson) ? new List<CartItem>() : JsonConvert.DeserializeObject<List<CartItem>>(cartJson);
        }

        private void ClearCart()
        {
            HttpContext.Session.Remove("cart");
        }

        
    }
}
