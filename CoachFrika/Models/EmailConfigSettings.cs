using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoachFrika.Models
{
    public class EmailConfigSettings
    {
        public string? DisplayName { get; set; }
        public string? From { get; set; } 
        public string[] MailTo { get; set; } = Array.Empty<string>();
        public string? SmtpServer { get; set; }
        public int Port { get; set; }
        public string? UserName { get; set; } 
        public string? Password { get; set; }
        public bool UseSSL { get; set; }
        public bool UseStartTls { get; set; }
        public string? ContactTopic { get; set; }
        public string? RequestTopic { get; set; }
        public string? NotificationTopic { get; set; }
    }
    public class CloudinarySettings
    {
        public string? CloudName { get; set; }
        public string? ApiKey { get; set; }
        public string? ApiSecret { get; set; }
    }

}
