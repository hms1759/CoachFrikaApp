using AutoMapper;
using CoachFrika.APIs.Domin.IServices;
using CoachFrika.APIs.ViewModel;
using CoachFrika.Common;
using CoachFrika.Common.AppUser;
using CoachFrika.Common.Extension;
using CoachFrika.Extensions;
using CoachFrika.Models;
using CoachFrika.Services;
using coachfrikaaaa.APIs.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.RegularExpressions;
using static CoachFrika.Common.LogingHandler.JwtServiceHandler;

namespace CoachFrika.APIs.Domin.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<CoachFrikaUsers> _userManager;
        private readonly SignInManager<CoachFrikaUsers> _signInManager;
        private readonly IJwtService _jwtService;
        public readonly IEmailService _emailService;
        public readonly IWebHelpers _webHelpers;
        private readonly IMapper _mapper;
        public AccountService(UserManager<CoachFrikaUsers> userManager, SignInManager<CoachFrikaUsers> signInManager, IJwtService jwtService, IEmailService emailService, IWebHelpers webHelpers, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _emailService = emailService;
            _webHelpers = webHelpers;
            _mapper = mapper;
        }

        public async Task<BaseResponse<LoginDetails>> Login(LoginDto login)
        {
            var res = new BaseResponse<LoginDetails>();
            res.Status = true;
            try
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
                var details = new LoginDetails();
                details.Token = await _jwtService.GenerateToken(user, roles);
                details.Roles = roles.ToList();
                details.Profile = _mapper.Map<ProfileDto>(user);



                res.Data = details;
                return res;
            }

            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = false;
                return res;

            }
        }
        public async Task<BaseResponse<SignpUpDto>> SignUp(SignpUpDto model)
        {
            var res = new BaseResponse<SignpUpDto>();
            res.Status = true;
            try
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
                        throw new ArgumentException("Phone number must be in the format: 0800 000 0000");
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
                    FullName = model.FullName,
                    Role = model.isCoach ? 1 : 0,
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
                res.Data = model;
                return res;
            }

            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = false;
                return res;

            }
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
        public bool ValidatePhoneNumber(string phoneNumber)
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

        public async Task<BaseResponse<string>> ForgetPassword(string email, string logoUrl)
        {
            var res = new BaseResponse<string>();
            res.Status = true;
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    throw new ArgumentException("User not found.");
                }

                var defaultPassword = GeneratePassword();

                var resetResult = await _userManager.RemovePasswordAsync(user);
                if (!resetResult.Succeeded)
                {
                    throw new InvalidOperationException("Failed to remove the old password.");
                }

                var result = await _userManager.AddPasswordAsync(user, defaultPassword);
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException("Failed to set the new password.");
                }

                // Optionally send the password via email
                await SendPasswordResetEmail(user, defaultPassword, logoUrl);
                res.Message = "Successful";
                return res;
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = false;
                return res;

            }
        }

        private async Task SendPasswordResetEmail(CoachFrikaUsers user, string newPassword, string logoUrl)
        {
            // Use an email service (SMTP, SendGrid, etc.) to send the new password to the user
            var subject = "Your password has been reset";
            var body = $"Your password has been reset. Your new password is: {newPassword}";

            var bodyTemplate = await _emailService.ReadTemplate("forgetPassword");
            //inserting variable
            var messageToParse = new Dictionary<string, string>
                    {
                        { "{Fullname}", user.FullName},
                        { "{Message}", body},
                        { "{logo}", logoUrl},
                    };

            //  email notification
            var messageBody = bodyTemplate.ParseTemplate(messageToParse);
            var message = new Message(new List<string> { user.Email }, subject, messageBody);

            await _emailService.SendEmail(message);
        }

        public string GeneratePassword(int length = 12)
        {
            // Character sets for password generation
            const string lowerCaseChars = "abcdefghijklmnopqrstuvwxyz";
            const string upperCaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string digits = "0123456789";
            const string specialChars = "!@#$%^&*()-_+=<>?";

            // Ensure the password meets the minimum length of 8 characters
            if (length < 8)
            {
                throw new ArgumentException("Password length must be at least 8 characters.");
            }

            var random = new Random();

            // Ensure that we have at least one character from each category
            var password = new StringBuilder();
            password.Append(lowerCaseChars[random.Next(lowerCaseChars.Length)]);
            password.Append(upperCaseChars[random.Next(upperCaseChars.Length)]);
            password.Append(digits[random.Next(digits.Length)]);
            password.Append(specialChars[random.Next(specialChars.Length)]);

            // Fill the rest of the password length with random characters from all sets
            var allChars = lowerCaseChars + upperCaseChars + digits + specialChars;
            for (int i = password.Length; i < length; i++)
            {
                password.Append(allChars[random.Next(allChars.Length)]);
            }

            // Shuffle the characters to randomize the order
            var shuffledPassword = password.ToString()
                .OrderBy(c => random.Next())
                .ToArray();

            return new string(shuffledPassword);
        }

        public async Task<BaseResponse<string>> ChangePassword(ChangePasswordDto model)
        {
            var res = new BaseResponse<string>();
            res.Status = true;
            try
            {
                var email = _webHelpers.CurrentUser();
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    throw new ArgumentException("User not found.");
                }

                var result = await _signInManager.PasswordSignInAsync(email, model.OldPassword, false, false);
                if (!result.Succeeded)
                {
                    throw new NotImplementedException("Invalid credentials");
                }
                var resetResult = await _userManager.RemovePasswordAsync(user);
                if (!resetResult.Succeeded)
                {
                    throw new InvalidOperationException("Failed to remove the old password.");
                }

                var resultss = await _userManager.AddPasswordAsync(user, model.NewPassword);
                if (!resultss.Succeeded)
                {
                    throw new InvalidOperationException("Failed to set the new password.");
                }

                res.Message = "Successful";
                return res;
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = false;
                return res;

            }
        }

    }
}
