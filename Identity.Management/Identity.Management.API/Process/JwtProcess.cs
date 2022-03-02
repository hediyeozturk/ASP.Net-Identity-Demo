using Identity.Management.API.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.Management.API.Process
{
    public class JwtProcess
    {
        private SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        IConfiguration _config;
        public JwtProcess(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IConfiguration config)
        {
            _signInManager = signInManager;
            _config = config;
            _userManager = userManager;

        }

        private IdentityUser Authenticate(UserLogin userLogin)
        {
            //_signInManager.SignInAsync(userLogin, isPersistent: false);
            var currentUser = _userManager.FindByNameAsync(userLogin.UserName).Result;

            //var currentUser = UserConstants.Users.FirstOrDefault(o => o.UserName.ToLower() == userLogin.UserName.ToLower()
            //         && o.Password == userLogin.Password);
            if (currentUser != null)
            {
                return currentUser;
            }

            return null;
        }

        private string Generate(IdentityUser user)
        {
            var identityResul = _signInManager.SignInAsync(user, isPersistent: false);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                //new Claim(ClaimTypes.GivenName, user.GivenName),
                //new Claim(ClaimTypes.Surname, user.Surname),
                //new Claim(ClaimTypes.Role, user),

            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                                            _config["Jwt:Audience"],
                                            claims,
                                            expires: DateTime.Now.AddMinutes(15),
                                            signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateToken(UserLogin userLogin)
        {
            IdentityUser user = Authenticate(userLogin);
            if (user != null)
            {
                string token = Generate(user);
                return token;
            }

            return null;

        }
    }
}
