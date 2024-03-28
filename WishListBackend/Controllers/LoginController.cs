using Microsoft.AspNetCore.Mvc;
using WishListBackend.Utils.Interfaces;
using WishListBackend.Views;

namespace WishListBackend.Controllers
{
    [ApiController]
    [Route("api/")]
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


        [HttpPost("login")]
        public IActionResult LogIn(LoginModel userData)
        {
            _userService.TryDeleteExpiredUser(userData.Email);
            var user = _userService.FindUserByEmail(userData.Email);

            if (user == null)
            {
                return BadRequest("Wrong email.");
            }

            if(!_userService.ComparePassword(user.EncryptedPassword, userData.Password))
            {
                return Unauthorized("Wrong password.");
            }

            if(user.ExpirationDate != null) 
            {
                return Unauthorized("Invalid Authentication");
            }

            var accessToken = _jwtLoginService.CreateLoginJwt(user);

            return Ok(accessToken);
        }
    }
}
