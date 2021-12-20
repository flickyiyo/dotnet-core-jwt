using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using courses_platform.Dtos;
using courses_platform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
//using courses_platform.Models;

namespace courses_platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        public CoursesDbContext _context;
        private readonly IConfiguration _config;
        public AuthenticationController(CoursesDbContext context, IConfiguration config)
        {
            this._config = config;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterUserDto request)
        {
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User{
                PasswordSalt = passwordSalt,
                Password = passwordHash,
                Username = request.Username,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneRegionCode = request.PhoneRegionCode,
                Phone = request.Phone,
            };

            try
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return Created($"/users/{user.Id}", new {
                    Id = user.Id
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Could Not create user");
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UserLoginDto request)
        {
            var foundUser = _context.Users.FirstOrDefault(u => u.Email == request.Email);

            if (foundUser != null)
            {
                if (VerifyPasswordHash(request.Password, foundUser.Password, foundUser.PasswordSalt))
                {
                    return Ok(new { token = CreateToken(foundUser) });
                }
            }

            return NotFound();
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA256(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA256())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.Name, user.Username),
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["JwtSettings:SignKey"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}