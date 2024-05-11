using AuthenticationMicroservice.Data;
using AuthenticationMicroservice.Models;
using Azure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;

namespace AuthenticationMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserDbContext dbContext;
        public static User user = new User();

        public AuthController(IConfiguration configuration, UserDbContext dbContext)
        {
            _configuration = configuration;
            this.dbContext = dbContext;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(AddUserRequest request)
        {
            if (request.password != null)
            {
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.password);

                user.userName = request.userName;
                user.password = passwordHash;
                user.role = request.role;

                await dbContext.Users.AddAsync(user);
                await dbContext.SaveChangesAsync();
                return Ok(user);
            }

            return NotFound();
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(LoginUser request)
        {
            User user = dbContext.Users
                .FirstOrDefault(u => u.userName == request.userName);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.password, user.password))
            {
                return Unauthorized("Invalid username or password");
            }

            var token = CreateToken(user);

            return Ok(new { token });
        }

        //[HttpDelete("delete")]
        //public async Task<ActionResult> Delete(String userName)
        //{
        //    var user = dbContext.Users.FirstOrDefault(u => u.userName == userName && us.);

        //    return Ok(user);
        //}

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.userName),
                new Claim(ClaimTypes.Role, user.role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var Token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(Token);

            return jwt;
        }

        //[HttpGet]
        //[Route("{username}")]
        //public async Task<IActionResult> UsernameExists(string username)
        //{
        //    // Implement logic to check username existence in your authentication database
        //    var exists = await UsernameExistsInAuthService(username);
        //    return Ok(exists);
        //}

        //private async Task<string> UsernameExistsInAuthService(string username)
        //{
        //    var exists = await dbContext.Users.AnyAsync(user => user.userName == username);
        //    return exists.ToString();
        //}

    }
}
