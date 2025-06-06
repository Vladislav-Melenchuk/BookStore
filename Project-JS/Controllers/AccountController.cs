using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_JS.Data;
using Project_JS.Models;

namespace Project_JS.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<User> _hasher = new PasswordHasher<User>();

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (_context.Users.Any(u => u.Username == model.Username))
            {
                ModelState.AddModelError(nameof(model.Username), "Користувач із таким ім'ям вже існує.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new User
            {
                Username = model.Username,
                PhoneNumber = model.PhoneNumber,
                IsAdmin = false
            };

            user.Password = _hasher.HashPassword(user, model.Password);

            _context.Users.Add(user);
            _context.SaveChanges();

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("IsAdmin", user.IsAdmin.ToString());

            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(7),
                HttpOnly = true,
                Secure = HttpContext.Request.IsHttps,
                SameSite = SameSiteMode.Strict
            };

            Response.Cookies.Append("UserId", user.Id.ToString(), cookieOptions);
            Response.Cookies.Append("Username", user.Username, cookieOptions);
            Response.Cookies.Append("IsAdmin", user.IsAdmin.ToString(), cookieOptions);

            TempData["Username"] = user.Username;
            TempData["IsAdmin"] = user.IsAdmin.ToString();
            return RedirectToAction("Guest", "Home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _context.Users.SingleOrDefault(u => u.Username == model.Username);
            if (user != null)
            {
                var result = _hasher.VerifyHashedPassword(user, user.Password, model.Password);
                if (result == PasswordVerificationResult.Success)
                {
                    HttpContext.Session.SetInt32("UserId", user.Id);
                    HttpContext.Session.SetString("Username", user.Username);
                    HttpContext.Session.SetString("IsAdmin", user.IsAdmin.ToString());

                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = HttpContext.Request.IsHttps,
                        SameSite = SameSiteMode.Strict
                    };

                    cookieOptions.Expires = model.RememberMe
                        ? DateTime.Now.AddDays(7)
                        : DateTime.Now.AddHours(1);

                    Response.Cookies.Append("UserId", user.Id.ToString(), cookieOptions);
                    Response.Cookies.Append("Username", user.Username, cookieOptions);
                    Response.Cookies.Append("IsAdmin", user.IsAdmin.ToString(), cookieOptions);

                    TempData["Username"] = user.Username;
                    TempData["IsAdmin"] = user.IsAdmin.ToString();
                    return RedirectToAction("Guest", "Home");
                }
            }

            ViewBag.ErrorMessage = "Невірне ім’я користувача або пароль.";
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            Response.Cookies.Delete("UserId");
            Response.Cookies.Delete("Username");
            Response.Cookies.Delete("IsAdmin");
            TempData.Clear();

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Profile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login");

            var user = _context.Users.Find(userId);
            if (user == null) return RedirectToAction("Login");

            return View(user);
        }

        [HttpPost]
        public IActionResult Profile(string email)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login");

            var user = _context.Users.Find(userId);
            if (user == null) return RedirectToAction("Login");

            if (!string.IsNullOrWhiteSpace(email))
            {
                user.Email = email.Trim();
                _context.SaveChanges();
                TempData["Success"] = "Email успішно оновлено!";
            }
            else
            {
                TempData["Error"] = "Email не може бути порожнім.";
            }

            return View(user);
        }


        [HttpGet]
        public IActionResult EditProfile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login");

            var user = _context.Users.Find(userId);
            if (user == null)
                return RedirectToAction("Login");

            var model = new EditProfileViewModel
            {
                Username = user.Username,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult EditProfile(EditProfileViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login");

            var user = _context.Users.Find(userId);
            if (user == null)
                return RedirectToAction("Login");

            
            user.Username = model.Username;
            user.PhoneNumber = model.PhoneNumber;
            user.Email = model.Email;

            try
            {
                _context.SaveChanges();
                TempData["Success"] = "Профіль успішно оновлено!";
                return RedirectToAction("Profile");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Помилка при оновленні профілю: {ex.Message}");
                return View(model);
            }
        }


        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(ChangePSWDViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login");

            var user = _context.Users.Find(userId);
            if (user == null)
                return RedirectToAction("Login");

            var verify = _hasher.VerifyHashedPassword(user, user.Password, model.OldPassword);
            if (verify != PasswordVerificationResult.Success)
            {
                ModelState.AddModelError("", "Старий пароль введено неправильно.");
                return View(model);
            }

            
            user.Password = _hasher.HashPassword(user, model.NewPassword);

            try
            {
                _context.SaveChanges();
                TempData["Success"] = "Пароль успішно змінено!";
                return RedirectToAction("Profile");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Помилка при оновленні пароля: " + ex.Message);
                return View(model);
            }
        }

       

        public IActionResult MyOrders()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login");

            var orders = _context.Orders
                .Include(o => o.Items) 
                .Where(o => o.UserId == userId)
                .ToList();

            return View(orders);
    }

}
}
