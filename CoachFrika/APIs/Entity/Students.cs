using CoachFrika.Common.Enum;
using coachfrikaaaa.Common;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace coachfrikaaaa.APIs.Entity
{
    public class Students : BaseEntity
    {
        public ClassRoomEnum? ClassId { get; set; }
        public long? StudentNumber { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? ParentName { get; set; }
        public string? Class { get; set; }
        public string? ParentPhoneNumber { get; set; }
        public string? TeachersId { get; set; }
    }
}
