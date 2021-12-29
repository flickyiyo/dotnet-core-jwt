namespace courses_platform.Dtos
{
    [Serializable]
    public class JwtAuthResponse
    {
        public string Token { get; set; }
        public DateTime ExpiresIn { get; set; }
    }
}