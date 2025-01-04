
using CoachFrika.APIs.Domin.IServices;
using CoachFrika.APIs.ViewModel;
using CoachFrika.Common;
using CoachFrika.Common.AppUser;
using CoachFrika.Common.AutoMapper;
using CoachFrika.Common.Extension;
using CoachFrika.Extensions;
using CoachFrika.Models;
using CoachFrika.Services;
using coachfrikaaaa.APIs.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
        private readonly ICloudinaryService _cloudinaryService;
        private readonly UiSiteConfigSettings _uiSite;
        public AccountService(UserManager<CoachFrikaUsers> userManager, SignInManager<CoachFrikaUsers> signInManager,
            IJwtService jwtService, IEmailService emailService, IOptions<UiSiteConfigSettings> uiSite, IWebHelpers webHelpers, ICloudinaryService cloudinaryService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _emailService = emailService;
            _webHelpers = webHelpers;
            _uiSite = uiSite.Value;
            _cloudinaryService = cloudinaryService;
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
                    res.Message = "User not found.";
                    res.Status = false;
                    return res;
                }
                var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, false, false);
                if (!result.Succeeded)
                {
                    res.Message = "Invalid credentials";
                    res.Status = false;
                    return res;
                }

                // Get the roles of the user
                var roles = await _userManager.GetRolesAsync(user);
                var details = new LoginDetails();
                details.Token = await _jwtService.GenerateToken(user, roles);

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
                    res.Message = "An account with this email already exists.";
                    res.Status = false;
                    return res;
                }

                // Validate phone number format
                if (!string.IsNullOrEmpty(model.PhoneNumber))
                {
                    var phoneNumberValid = Validators.ValidatePhoneNumber(model.PhoneNumber);
                    if (!phoneNumberValid)
                    {
                        res.Message = "Phone number must be in the format: 0800 000 0000";
                        res.Status = false;
                        return res;
                    }

                    // Check if the phone number already exists
                    var existingUserByPhone = await _userManager.Users
                        .FirstOrDefaultAsync(u => u.PhoneNumber == model.PhoneNumber);
                    if (existingUserByPhone != null)
                    {
                        res.Message = "An account with this phone number already exists.";
                        res.Status = false;
                        return res;
                    }
                }

                // Validate email format
                var emailValid = Validators.ValidateEmail(model.Email);
                if (!emailValid)
                {
                    res.Message = "Invalid email format.";
                    res.Status = false;
                    return res;
                }

                // Validate password using a regular expression
                var passwordValid = Validators.ValidatePassword(model.Password);
                if (!passwordValid)
                {
                    res.Message = "Password must be at least 8 characters long, include at least one uppercase letter, one digit, and one special character.";
                    res.Status = false;
                    return res;
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
                model.Password = null;
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


        public async Task<BaseResponse<string>> ForgetPassword(string email, string logoUrl)
        {
            var res = new BaseResponse<string>();
            res.Status = true;
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    res.Message = "User not found.";
                    res.Status = false;
                    return res;
                }

                var defaultPassword = GeneratePassword();

                var resetResult = await _userManager.RemovePasswordAsync(user);
                if (!resetResult.Succeeded)
                {
                    res.Message = "Failed to remove the old password.";
                    res.Status = false;
                    return res;
                }

                var result = await _userManager.AddPasswordAsync(user, defaultPassword);
                if (!result.Succeeded)
                {
                    res.Message = "Failed to set the new password.";
                    res.Status = false;
                    return res;
                }

                user.IsPasswordDefault = true;
                await _userManager.UpdateAsync(user);
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
            var subject = "Your password has been reset";
            var body = $"Your password has been reset. Your default password is: {newPassword} <br> click here <a href={_uiSite.SiteUrl}auth/reset-password>here</a> to reset your password  ";

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
            const string specialChars = "!@$?";

            // Predefined words for password generation (like "HomeComing")
            var predefinedWords = new string[] { "HomeComing", "WinterBreak", "SummerFun", "Spring2024" };

            var random = new Random();

            // Start by picking a random predefined word
            string password = predefinedWords[random.Next(predefinedWords.Length)];

            // Add a random special character to enhance complexity
            password += specialChars[random.Next(specialChars.Length)];

            // Add a random digit at the end
            password += digits[random.Next(digits.Length)];

            // If the password length is still less than the desired length, fill with random characters
            var allChars = lowerCaseChars + upperCaseChars + digits + specialChars;
            while (password.Length < length)
            {
                password += allChars[random.Next(allChars.Length)];
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
            res.Status = false;
            try
            {
                var email = _webHelpers.CurrentUser();
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    res.Message = "User not found.";
                    return res;
                }

                var result = await _signInManager.PasswordSignInAsync(email, model.OldPassword, false, false);
                if (!result.Succeeded)
                {
                    res.Message = "Invalid credentials";
                    return res;
                }
                var resetResult = await _userManager.RemovePasswordAsync(user);
                if (!resetResult.Succeeded)
                {
                    res.Message = "Failed to remove the old password.";
                    return res;
                }

                var resultss = await _userManager.AddPasswordAsync(user, model.NewPassword);
                if (!resultss.Succeeded)
                {
                    res.Message = "Failed to set the new password.";
                    return res;
                }
                res.Status = true;
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

        public async Task<BaseResponse<string>> UploadFile(ProfileImgUpload model)
        {
            var res = new BaseResponse<string>();
            res.Status = true;
            try
            {
                if (model.ProfileImage == null)
                {

                    res.Message = "payload can not be null";
                    res.Status = false;
                    SentrySdk.CaptureMessage("payload can not be null", level: SentryLevel.Error);
                    return res;

                }

                SentrySdk.CaptureMessage($"Starting file upload. File name: {model.ProfileImage.FileName}, File size: {model.ProfileImage.Length} bytes.", level: SentryLevel.Info);

                if (model.ProfileImage == null || model.ProfileImage.Length == 0)
                {
                    res.Message = "No file uploaded.";
                    res.Status = false;
                    return res;
                }
                if (model.ProfileImage.Length > 0 && model.ProfileImage.Length <= 204800)
                {

                    var email = _webHelpers.CurrentUser();
                    SentrySdk.CaptureMessage($"Current user: {email}", level: SentryLevel.Info);

                    if (string.IsNullOrEmpty(email))
                    {
                        res.Message = "Current user not found.";
                        res.Status = false;
                        return res;
                    }
                    var user = await _userManager.FindByEmailAsync(email);
                    if (user == null)
                    {
                        res.Message = "User not found.";
                        res.Status = false;
                        return res;
                    }

                    var fileUrl = await _cloudinaryService.UploadFileAsync(model.ProfileImage);
                    if (fileUrl != null)
                    {
                        user.ProfileImageUrl = fileUrl;
                        await _userManager.UpdateAsync(user);
                        res.Data = fileUrl;
                        return res;
                    }
                    res.Message = "Error uploading file.";
                    res.Status = false;
                    return res;
                }
                else
                {
                    res.Message = "file most not be higher than 200KB";
                    res.Status = false;
                    return res;
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureMessage($"Message :{ex.Message},StackTrace:{ex.StackTrace}", level: SentryLevel.Info);
                res.Message = ex.Message;
                res.Status = false;
                return res;

            }

        }

        public async Task<BaseResponse<string>> ResetPassword(ResetPasswordDto model)
        {
            var res = new BaseResponse<string>();
            res.Status = false;
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    res.Message = "User not found.";

                    return res;
                }

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.DefaultPassword, false, false);
                if (!result.Succeeded)
                {
                    res.Message = "Kindly use the code in your email has default Password";
                    return res;
                }
                var resetResult = await _userManager.RemovePasswordAsync(user);
                if (!resetResult.Succeeded)
                {
                    res.Message = "Failed to remove the old password.";
                    return res;
                }

                var resultss = await _userManager.AddPasswordAsync(user, model.Password);
                if (!resultss.Succeeded)
                {
                    res.Message = "Failed to set the new password.";

                    return res;
                }

                res.Status = true;
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
