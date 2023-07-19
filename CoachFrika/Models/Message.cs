
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoachFrika.Models
{
    public class Message
    {
        public List<string> To { get; set; }
        // public string? From { get; set; }
        // public string? DisplayName { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }

        //constructor
        public Message(List<string> to, string subject, string? body = null)
        {
            To = to;
            // From = from;
            //DisplayName = displayName;
            Subject = subject;
            Body = body;

        }
    }
}
