﻿using CoachFrika.Common.Enum;
using coachfrikaaaa.Common;
using Microsoft.AspNetCore.Identity;

namespace coachfrikaaaa.APIs.Entity
{
    public class Schedule : BaseEntity
    {
        public string? Title { get; set; }
        public Subscriptions? Focus { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? MeetingLink { get; set; }
        public string? CoachId { get; set; }
        public bool CoachAttended { get; set; }
        public bool TeacherAttended { get; set; }
        public CoachFrikaUsers? Coach { get; set; }
    }
}
