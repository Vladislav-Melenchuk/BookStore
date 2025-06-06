using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Project_JS.Models;

namespace Project_JS.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartApiController : ControllerBase
    {
        [HttpPost("save")]
        public IActionResult SaveCart([FromBody] List<CartItem> items)
        {
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(items));
            return Ok();
        }
    }
}
