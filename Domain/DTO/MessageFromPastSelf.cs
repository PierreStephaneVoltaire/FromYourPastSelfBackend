using System;

namespace infrastructure
{

    public class MessageFromPastSelfDto
    {
        public string To { get; set; }
        public string Subject{ get; set; }
        public string Body { get; set; }
        public int when { get; set; }
        public bool IsBodyHtml{ get; set; }
    }
}