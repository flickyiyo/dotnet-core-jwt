using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace courses_platform.Models
{
    public class UserLesson
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }
        public int? Minute { get; set; }
        public int? Second { get; set; }
        public bool IsFinished { get; set; }
    }
}