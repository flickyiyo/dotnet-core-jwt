using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using courses_platform.Models;
using courses_platform.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace courses_platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly CoursesDbContext _context;
        private readonly ILogger _logger;

        public CourseController(CoursesDbContext context, ILogger<CourseController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses()
        {
            var courses = _context.Courses.ToList();
            if (courses == null)
            {
                return NotFound();
            }
            return Ok(courses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourseById(string id)
        {
            var course = _context.Courses.Single(c => c.Id == id);

            if (course == null)
            {
                return NotFound();
            }
            return Ok(course);
        }

        [HttpPost("")]
        // [Authorize(Roles = (string)Role.TEACHER)]
        [Authorize(Roles = "TEACHER,ADMIN,MODERATOR")]
        // [Authorize(Roles = "MODERATOR")]
        public async Task<ActionResult<CourseDto>> PostCourse(CourseDto model)
        {
            var course = new Course{
                Name = model.Name,
                Slug = model.Slug
            };
            
            var tsx = _context.Database.BeginTransaction();
            try
            {
                 _context.Courses.Add(course);
                 _context.SaveChanges();
                
                tsx.Commit();

                return Ok(course);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogInformation(ex.Message);
                tsx.Rollback();
            }
            
            return StatusCode(500, new { Message = "Could Not Create Course" });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "TEACHER,ADMIN,MODERATOR")]
        public async Task<IActionResult> PutCourse(int id, Course model)
        {
            

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CourseDto>> DeleteCourseById(int id)
        {
            // TODO: Your code here
            await Task.Yield();

            return null;
        }
    }
}