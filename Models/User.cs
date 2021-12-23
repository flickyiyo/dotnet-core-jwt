using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace courses_platform.Models
{
    public class User : IdentityUser
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        // public string Email { get; set; }
        // public string UserName { get; set; }
        // public byte[] Password { get; set; }
        public byte[] PasswordSalt { get; set; }
        // public string FirstName { get; set; }
        // public string LastName { get; set; }
        public string Phone { get; set; }
        public string PhoneRegionCode { get; set; }
        // // public string Role { get; set; }
        // public ICollection<UserLesson> UserLessons { get; set; }
    }
}