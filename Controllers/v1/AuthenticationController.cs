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
using courses_platform.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace courses_platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        public CoursesDbContext _context;
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthenticationController(CoursesDbContext context,
                                        IConfiguration config,
                                        UserManager<User> userManager,
                                        SignInManager<User> signInManager)
        {
            _context = context;
            _config = config;
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        // public AuthenticationController(CoursesDbContext context,
        //                                 IConfiguration config)
        // {
        //     _context = context;
        //     _config = config;
        //     // this.userManager = userManager;
        //     // this.signInManager = signInManager;
        // }

        [HttpPost("register")]
        // [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterUserDto request)
        {
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User
            {
                PasswordSalt = passwordSalt,
                PasswordHash = Encoding.UTF8.GetString(passwordHash),
                UserName = request.Username,
                // FirstName = request.FirstName,
                // LastName = request.LastName,
                Email = request.Email,
                PhoneRegionCode = request.PhoneRegionCode,
                Phone = request.Phone,
                // Roles = new List<Role>{ Role.FREE_STUDENT }
            };

            try
            {
                Console.WriteLine(user.Email);
                var result = await _userManager.CreateAsync(user, user.PasswordHash);
                
                if (result.Succeeded)
                {
                    Console.WriteLine(Url.Action(user.Id));
                    return Created(Url.Action(user.Id), new UserDto{ Id = user.Id, Email = user.Email });
                }
                var errorMessages = new List<string>();
                foreach(var err in result.Errors)
                {
                    errorMessages.Add(err.Description);
                }
                return StatusCode(400, errorMessages);
            }
            catch (DbUpdateException ex)
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
                if (VerifyPasswordHash(request.Password, Encoding.UTF8.GetBytes(foundUser.PasswordHash), foundUser.PasswordSalt))
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
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            // foreach (Role role in user.Roles)
            // {
            claims.Add(new Claim(ClaimTypes.Role, Role.ADMIN.ToString()));
            // }

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