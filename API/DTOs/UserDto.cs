
namespace API.DTOs
{
    public class UserDto
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public string PhotoUrl { get; set; } = "empty";

        public string KnownAs { get; set; }
    }
}