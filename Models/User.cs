using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace courses_platform.Models
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Email { get; set; }
        public string Username { get; set; }
        public byte[] Password { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string PhoneRegionCode { get; set; }
    }
}