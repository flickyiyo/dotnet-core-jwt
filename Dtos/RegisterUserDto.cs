namespace courses_platform.Dtos
{
    public class RegisterUserDto
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string PhoneRegionCode { get; set; }
    }
}