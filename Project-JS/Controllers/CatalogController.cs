using Microsoft.AspNetCore.Mvc;
using Project_JS.Data;


namespace Project_JS.Controllers
{
    public class CatalogController : Controller
    {
        private readonly AppDbContext _context;
          public CatalogController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var books = _context.Books.ToList();
            return View(books);
        }

    }
}
