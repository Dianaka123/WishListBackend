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
        private readonly IJwtLoginService _jwtLoginService;
        private readonly IUserService _userService;

        public LoginController(
            ILogger<LoginController> logger,
            IUserService userService,
            IJwtLoginService jwtLoginService)
        {
            _logger = logger;
            _userService = userService;
            _jwtLoginService = jwtLoginService;
        }

        public record LogInData(string Email, string Password);

        [HttpPost(Name = "login")]
        public IActionResult LogIn(LogInData userData)
        {
            var user = _userService.FindUserByEmail(userData.Email);

            if (user == null)
            {
                return BadRequest("Wrong email.");
            }

            if(!_userService.ComparePassword(user.EncryptedPassword, userData.Password))
            {
                return BadRequest("Wrong password.");
            }

            var accessToken = _jwtLoginService.CreateJwt(user.Id.ToString());

            return Ok(accessToken);
        }
    }
}
