using Identity.Management.API.Model;
using Identity.Management.API.Process;
using Identity.Management.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Management.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public UserManager<IdentityUser> _userManager { get; }
        public SignInManager<IdentityUser> _signInManager { get; }
        private IConfiguration _config;

        JwtProcess jwt;
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            jwt = new JwtProcess(_signInManager, _userManager, _config );
        }


        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterViewModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            var user = new IdentityUser
            {
                Email = model.Email,
                UserName = model.Email

            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return Ok("Registered Successfully");
            }

            return Unauthorized();
            //return exc("USer not found");

        }


        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            if (userLogin == null)
            {
                return BadRequest();
            }
            else
            {
                string token = jwt.GenerateToken(userLogin);
                if (token != null)
                {
                    return Ok(token);
                }

                return NotFound("USer not found");
            }
        }
    }
}
