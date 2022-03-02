using Identity.Management.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.Management.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;

        public LoginController(IConfiguration config)
        {
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            if (userLogin == null)
            {
                return BadRequest();
            }
            else
            {
                IdentityUser user = Authenticate(userLogin);
                if (user != null)
                {
                    string token = Generate(user);
                    return Ok(token);
                }

                return NotFound("USer not found");
            }
        }

        private IdentityUser Authenticate(UserLogin userLogin)
        {
            
            //var currentUser = UserConstants.Users.FirstOrDefault(o => o.UserName.ToLower() == userLogin.UserName.ToLower()
            //         && o.Password == userLogin.Password);
            //if (currentUser != null)
            //{
            //    return currentUser;
            //}

            return null;
        }

        private string Generate(IdentityUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
                //new Claim(ClaimTypes.GivenName, user.GivenName),
                //new Claim(ClaimTypes.Surname, user.Surname),
                //new Claim(ClaimTypes.Role, user.Role),

            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                                            _config["Jwt:Audience"],
                                            claims,
                                            expires: DateTime.Now.AddMinutes(15),
                                            signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
