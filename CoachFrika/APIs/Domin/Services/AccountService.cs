using CoachFrika.APIs.Domin.IServices;
using CoachFrika.APIs.ViewModel;
using CoachFrika.Common.AppUser;
using coachfrikaaaa.APIs.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using static CoachFrika.Common.LogingHandler.JwtServiceHandler;

namespace CoachFrika.APIs.Domin.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<CoachFrikaUsers> _userManager;
        private readonly SignInManager<CoachFrikaUsers> _signInManager;
        private readonly IJwtService _jwtService;

        public AccountService(UserManager<CoachFrikaUsers> userManager, SignInManager<CoachFrikaUsers> signInManager, IJwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
        }

        public async Task<string> Login(LoginDto login)
        {

            // If sign-in is successful, generate JWT token
            var user = await _userManager.FindByNameAsync(login.Email);
            if (user == null)
            {
                throw new NotImplementedException("User not found.");
            }
            var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, false, false);
            if (!result.Succeeded)
            {
                throw new NotImplementedException("Invalid credentials");
            }
            // Get the roles of the user
            var roles = await _userManager.GetRolesAsync(user);

            return await _jwtService.GenerateToken(user, roles);
        }
        public async Task<SignpUpDto> SignUp(SignpUpDto model)
        {
            // Check if the email already exists
            var existingUserByEmail = await _userManager.FindByEmailAsync(model.Email);
            if (existingUserByEmail != null)
            {
                throw new ArgumentException("An account with this email already exists.");
            }

            // Validate phone number format
            if (!string.IsNullOrEmpty(model.PhoneNumber))
            {
                var phoneNumberValid = ValidatePhoneNumber(model.PhoneNumber);
                if (!phoneNumberValid)
                {
                    throw new ArgumentException("Phone number must be in the format: 08068783985");
                }

                // Check if the phone number already exists
                var existingUserByPhone = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.PhoneNumber == model.PhoneNumber);
                if (existingUserByPhone != null)
                {
                    throw new ArgumentException("An account with this phone number already exists.");
                }
            }

            // Validate email format
            var emailValid = ValidateEmail(model.Email);
            if (!emailValid)
            {
                throw new ArgumentException("Invalid email format.");
            }

            // Validate password using a regular expression
            var passwordValid = ValidatePassword(model.Password);
            if (!passwordValid)
            {
                throw new ArgumentException("Password must be at least 8 characters long, include at least one uppercase letter, one digit, and one special character.");
            }

            // Create a new user
            var user = new CoachFrikaUsers
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                FirstName = model.FirstName,
                LastName = model.LastName,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // Assign Role
                await _userManager.AddToRoleAsync(user, model.isCoach ? AppRoles.Coach : AppRoles.Teacher);
            }
            else
            {
                // Return the error messages from Identity
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            return model;
        }

        // Helper method to validate password
        private bool ValidatePassword(string password)
        {
            // Password must be at least 8 characters long, with at least one uppercase letter,
            // one digit, one special character, and must be alphanumeric
            var passwordRegex = new Regex(@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");
            return passwordRegex.IsMatch(password);
        }

        // Helper method to validate phone number format (e.g., 08068783985)
        private bool ValidatePhoneNumber(string phoneNumber)
        {
            var phoneRegex = new Regex(@"^0\d{10}$");  // Starts with '0' followed by 10 digits
            return phoneRegex.IsMatch(phoneNumber);
        }

        // Helper method to validate email format
        private bool ValidateEmail(string email)
        {
            var emailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            return emailRegex.IsMatch(email);
        }
    }
}
