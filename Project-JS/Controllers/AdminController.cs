using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_JS.Data;
using Project_JS.Models;

namespace Project_JS.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AdminController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        
        [HttpGet]
        public IActionResult AddBook()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AdminOrders()
        {
            var orders = _context.Orders
                .Include(o => o.User) 
                .ToList();

            return View(orders);
        }


        [HttpPost]
        public async Task<IActionResult> AddBook(Book book, IFormFile ImageFile)
        {
            if (string.IsNullOrWhiteSpace(book.Title) || string.IsNullOrWhiteSpace(book.Author))
            {
                TempData["Error"] = "Будь ласка, заповніть назву та автора.";
                return View(book);
            }

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                book.ImagePath = "/uploads/" + fileName;
            }
            else
            {
                book.ImagePath = "/uploads/default.png"; 
            }

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Книга успішно додана!";
            return RedirectToAction("AdminPanel");
        }

        
        public IActionResult AdminPanel()
        {
            var books = _context.Books.ToList();
            return View(books);
        }

       
        [HttpGet]
        public IActionResult EditBook(int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

       
        [HttpPost]
        public async Task<IActionResult> EditBook(int id, Book updatedBook, IFormFile ImageFile)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            book.Title = updatedBook.Title;
            book.Author = updatedBook.Author;
            book.Price = updatedBook.Price;
            book.Category = updatedBook.Category;
            book.Description = updatedBook.Description;

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                book.ImagePath = "/uploads/" + fileName;
            }

            _context.Books.Update(book);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Книга оновлена.";
            return RedirectToAction("AdminPanel");
        }

        
        [HttpGet]
        public IActionResult DeleteBook(int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            _context.SaveChanges();

            TempData["Success"] = "Книга видалена.";
            return RedirectToAction("AdminPanel");
        }

        [HttpPost]
        public IActionResult UpdateOrderStatus(int orderId, string status)
        {
            var order = _context.Orders.Find(orderId);
            if (order == null)
                return NotFound();

            order.Status = status;
            _context.SaveChanges();
            HttpContext.Session.SetInt32("UserId", order.UserId);


            TempData["Success"] = $"Статус замовлення №{order.Id} оновлено.";
            return RedirectToAction("AdminOrders");
        }

        [HttpGet]
        public IActionResult AdminOrderDetails(int id)
        {
            var order = _context.Orders
                .Include(o => o.Items)
                .Include(o => o.User)
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
                return NotFound();

            return View(order);
        }



    }
}
