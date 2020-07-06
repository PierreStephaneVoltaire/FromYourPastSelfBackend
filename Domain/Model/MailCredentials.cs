using System;

namespace infrastructure
{
    public class MailCredentials
    {
        public MailCredentials(string email, string password)
        {
            this.email = email ?? throw new ArgumentNullException(nameof(email));
            this.password = password ?? throw new ArgumentNullException(nameof(password));
        }

        public string email { get; set; }
        public string password { get; set; }


    }
}