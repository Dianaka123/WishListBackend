using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WishListBackend.Models;
using WishListBackend.Other.Interfaces;
using WishListBackend.Utils.Interfaces;

namespace WishListBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IUserService _userService;
        private readonly IPasswordEncoder _passwordEncoder;

        public LoginController(
            ILogger<LoginController> logger,
            IUserService userService,
            IPasswordEncoder passwordEncoder)
        {
            _logger = logger;
            _userService = userService;
            _passwordEncoder = passwordEncoder;
        }

        public record LogInData(string email, string password);

        [HttpPost(Name = "LogIn")]
        public IActionResult LogIn(LogInData userData)
        {
            var user = _userService.FindUserByEmail(userData.email);

            if (user == null)
            {
                return BadRequest("Wrong email.");
            }

            if(!_userService.ComparePassword(user.EncryptedPassword, userData.password))
            {
                return BadRequest("Wrong password.");
            }

            return Ok();
        }
    }
}
