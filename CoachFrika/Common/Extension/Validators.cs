using System.Text.RegularExpressions;

namespace CoachFrika.Common.Extension
{
    public static class Validators
    {
        public static bool ValidateEmail(string email)
        {
            var emailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            return emailRegex.IsMatch(email);
        }
        // Helper method to validate password
        public static bool ValidatePassword(string password)
        {
            // Password must be at least 8 characters long, with at least one uppercase letter,
            // one digit, one special character, and must be alphanumeric
            var passwordRegex = new Regex(@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");
            return passwordRegex.IsMatch(password);
        }

        // Helper method to validate phone number format (e.g., 08068783985)
        public static bool ValidatePhoneNumber(string phoneNumber)
        {
            var phoneRegex = new Regex(@"^0\d{10}$");  // Starts with '0' followed by 10 digits
            return phoneRegex.IsMatch(phoneNumber);
        }

       
    }
}
