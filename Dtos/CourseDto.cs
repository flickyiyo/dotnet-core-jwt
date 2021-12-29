using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace courses_platform.Dtos
{
    public class CourseDto
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Slug { get; set; }
    }
}