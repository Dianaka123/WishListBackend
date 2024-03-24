using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WishListBackend.Models;
using WishListBackend.Other.Interfaces;
using WishListBackend.Utils.Interfaces;
using WishListBackend.Views;

namespace WishListBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IJwtService _jwtLoginService;
        private readonly IUserService _userService;

        public LoginController(
            ILogger<LoginController> logger,
            IUserService userService,
            IJwtService jwtLoginService)
        {
            _logger = logger;
            _userService = userService;
            _jwtLoginService = jwtLoginService;
        }


        [HttpPost(Name = "login")]
        public IActionResult LogIn(LoginModel userData)
        {
            var user = _userService.FindUserByEmail(userData.Email);

            if (user == null)
            {
                return BadRequest("Wrong email.");
            }

            if(!_userService.ComparePassword(user.EncryptedPassword, userData.Password))
            {
                return Unauthorized("Wrong password.");
            }

            if(!user.IsEmailConfirmed) 
            {
                return Unauthorized("Invalid Authentication");
            }

            var accessToken = _jwtLoginService.CreateLoginJwt(user);

            return Ok(accessToken);
        }
    }
}
