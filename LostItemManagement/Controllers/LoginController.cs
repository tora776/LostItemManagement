using Microsoft.AspNetCore.Mvc;
using LostItemManagement.Services;
using LostItemManagement.Models;

namespace LostItemManagement.Controllers
{
    [Route("[controller]")]
    public class LoginController : Controller
    {
        private readonly LoginService _service;

        public LoginController(DatabaseContext context)
        {
            var repository = new LoginRepository(context);
            _service = new LoginService(repository);
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View("~/Views/Home/login.cshtml");
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] LoginRequest request)
        {
            var token = _service.Authenticate(request.userId, request.password);
            if (token == null)
                return Unauthorized();

            return Json(new { token });
        }

        [HttpPost("checktoken")]
        public IActionResult CheckToken([FromBody] TokenRequest request)
        {
            if (!_service.IsTokenValid(request.Token))
                return Unauthorized();

            return Ok();
        }
    }

    public class LoginRequest
    {
        public string userId { get; set; }
        public string password { get; set; }
    }

    public class TokenRequest
    {
        public string Token { get; set; }
    }
}