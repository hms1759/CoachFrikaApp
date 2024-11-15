namespace CoachFrika.APIs.ViewModel
{
    public class SignpUpDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool isCoach { get; set; }
    }

    public class LoginDto
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
    public class LoginDetails
    {
        public string? Token { get; set; }
        public string? ExpiredTime { get; set; }
    }
}
