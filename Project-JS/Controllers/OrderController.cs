using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Project_JS.Data;
using Project_JS.Models;

namespace Project_JS.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Checkout(OrderFormViewModel model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");


            var cart = GetCart();
            if (cart.Count == 0)
            {
                TempData["Error"] = "Корзина пуста!";
                return RedirectToAction("Checkout");
            }

            if (!ModelState.IsValid)
                return View(model);

            var order = new Order
            {
                UserId = userId.Value,
                OrderDate = DateTime.Now,
                Status = "Очікує обробки",
                FullName = model.FullName,
                Address = model.Address,
                PhoneNumber = model.PhoneNumber,
                AdditionalNotes = model.AdditionalNotes,
                Items = cart.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    Title = item.Title
                }).ToList()
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            HttpContext.Session.Remove("Cart");

            return RedirectToAction("OrderSuccess");
        }

        public IActionResult OrderSuccess()
        {
            return View();
        }

        private List<CartItem> GetCart()
        {
            var cartStr = HttpContext.Session.GetString("Cart");
            return string.IsNullOrEmpty(cartStr)
                ? new List<CartItem>()
                : JsonConvert.DeserializeObject<List<CartItem>>(cartStr);
        }

        public IActionResult OrderDetails(int orderId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var order = _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product) 
                .FirstOrDefault(o => o.Id == orderId && o.UserId == userId);

            if (order == null)
                return NotFound();

            return View(order);
        }

    }

    public class CartItem
    {
        public int ProductId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
