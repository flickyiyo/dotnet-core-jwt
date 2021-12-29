using System.Security.Claims;
using courses_platform.Dtos;
using courses_platform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace plataforma_cursos_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        public CoursesDbContext _context { get; set; }
        private readonly UserManager<User> _userManager;

        public UserController(CoursesDbContext context)
        {
            _context = context;
        }

        public UserController(CoursesDbContext context, UserManager<User> userManager) : this(context)
        {
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetUserData()
        {
            var email = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email);
            
            if (email != null)
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == email.Value);
                if (user != null)
                {
                    return Ok(new UserDto(user));
                }
            }

            return NotFound();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUserData(UserDto userDto)
        {
            // var token = 
            // Url.Action("ConfirmEmail", "User", new { Id = userDto.Id, token = token});
            throw new NotImplementedException();
        }
    }
}