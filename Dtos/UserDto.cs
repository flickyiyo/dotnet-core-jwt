using courses_platform.Models;

namespace courses_platform.Dtos
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Phone { get; set; }
        public string PhoneRegionCode { get; set; }

        public UserDto(string id, string email, string username, string phone, string phoneRegionCode)
        {
            Id = id;
            Email = email;
            Username = username;
            Phone = phone;
            PhoneRegionCode = phoneRegionCode;
        }

        public UserDto(User user)
        {
            Id = user.Id;
            Email = user.Email;
            Username = user.UserName;
            Phone = user.Phone;
            PhoneRegionCode = user.PhoneRegionCode;
        }

        public UserDto()
        {
            
        }
    }
}