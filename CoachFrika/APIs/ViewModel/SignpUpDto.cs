using CoachFrika.Common.Enum;
using coachfrikaaaa.APIs.Entity;
using coachfrikaaaa.Common;
using System.Text.Json.Serialization;

namespace CoachFrika.APIs.ViewModel
{
    public class SignpUpDto
    {
        public string? FullName { get; set; }
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

    public class ChangePasswordDto
    {
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? ConfirmPassword { get; set; }
    }
    public class PublicCountDto
    {
        public int CoachesCount { get; set; }
        public int TeachersCount { get; set; }
        public int StudentCount { get; set; }
        public int SchoolCount { get; set; }
    }
    public class SubscriptionDto
    {
        public string? Email { get; set; }
    }
    public class ContactUsDto
    {
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Message { get; set; }
        [JsonIgnore]
        public string? logoUrl { get; set; }
    }
    public class SchoolEnrollmentDto
    {
        public string? SchoolName { get; set; }
        public string? SchoolAddress { get; set; }
        public string? ContactPersonEmail { get; set; }
        public string? ContactPersonName { get; set; }
        public string? ContactPersonPhoneNumber { get; set; }
        public int NumbersOfTeachers { get; set; }
        public Subscriptions? Subscriptions { get; set; }
        [JsonIgnore]
        public string? logoUrl { get; set; }
    }
}
