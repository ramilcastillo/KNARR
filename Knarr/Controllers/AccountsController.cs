using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Knarr.Controllers.Resources.Accounts;
using Knarr.Core.Models;

namespace Knarr.Controllers
{

    [Route("api/[controller]/[action]")]
    public class AccountsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;


        public AccountsController(IMapper mapper, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost]
        [ActionName("register")]
        public async Task<IActionResult> Register([FromBody]RegisterResource registerResource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var emailExist = await _userManager.FindByEmailAsync(registerResource.Email);

            if (emailExist != null)
                return BadRequest("Email already in use.");

            var user = new ApplicationUser
            {
                UserName = registerResource.Email,
                Email = registerResource.Email,
                FirstName = registerResource.FirstName,
                LastName = registerResource.LastName,
                Country = registerResource.Country,
                Address = registerResource.Address,
                State = registerResource.State,
                PhoneNumber = registerResource.PhoneNumber
            };
            var result = await _userManager.CreateAsync(user, registerResource.Password);
            if (result.Succeeded)
            {
                var roleAddresult = await _userManager.AddToRoleAsync(user, "Passenger");
                if (roleAddresult.Succeeded)
                {
                    return Ok(user);
                }
                return BadRequest("User created successfully but Role can not be created. Errors are: " + roleAddresult.Errors.ToString());
            }

            var sb = new StringBuilder();
            foreach (var error in result.Errors)
            {
                sb.Append(error.Description);
                sb.Append("\n");
            }
            return BadRequest(sb.ToString());
        }

        [HttpPost]
        [ActionName("login")]
        public async Task<Object> Login([FromBody] LoginResource login)
        {
            var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, false, false);
            if (result.Succeeded)
            {
                var appUser = _userManager.Users.SingleOrDefault(r => r.Email == login.Email);
                return GenerateJwtToken(login.Email, appUser);
            }
            return BadRequest(result.ToString());
        }

        private object GenerateJwtToken(string email, ApplicationUser user)
        {

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Email, email));
            var roles = _userManager.GetRolesAsync(user).Result.ToList();
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var expireDays = Convert.ToInt32(_configuration["JwtExpireDays"]);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(expireDays);

            var token = new JwtSecurityToken(
             issuer: _configuration["JwtIssuer"],
             audience: _configuration["JwtIssuer"],
             claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
