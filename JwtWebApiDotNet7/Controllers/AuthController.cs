using UploadFile.Models;
using UploadFile.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Upload_File.Models;

namespace UploadFile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private dbContext _dbcontext;
        public AuthController(IConfiguration configuration, IUserService userService,dbContext dbContext)
        {
            _configuration = configuration;
            _userService = userService;
            _dbcontext = dbContext;
        }



        [HttpPost("register")]
        public ActionResult<User> Register(UserDto request)
        {
            /*string passwordHash
                = BCrypt.Net.BCrypt.HashPassword(request.Password);*/
            string passwordHash
                = request.Password;
            User user = new User();
            user.Username = request.Username;
            if (string.IsNullOrEmpty(user.PasswordHash))
            {
                user.PasswordHash = passwordHash;
            }
            _dbcontext.Add(user);
            _dbcontext.SaveChanges();

            return Ok(user);
        }

        [HttpPost("login")]
        public ActionResult<User> Login(UserDto request)
        {
            User user = new User()
            {
                Username = request.Username,
                PasswordHash=request.Password,
            };

            if (!_dbcontext.Users.Any(u=>u==user))
            {
                return BadRequest("User not found.");
            }

            string token = CreateToken(user);

            return Ok(token);
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.Role, "User"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
