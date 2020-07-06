using System;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using Domain.Adapter;
using Hangfire;

namespace infrastructure
{
    public class GMail : Mail
    {
        private readonly string _email;
        private readonly string _password;

        public GMail(MailCredentials credentials)
        {
            _email = credentials.email ?? throw new ArgumentNullException(nameof(credentials.email));
            _password = credentials.password ?? throw new ArgumentNullException(nameof(credentials.password));
        }


        public string scheduleEmail(MessageFromPastSelf messageFromPastSelf)
        {
            return BackgroundJob.Schedule(
                () => sendEmail(messageFromPastSelf),
                TimeSpan.FromDays(messageFromPastSelf.when));
        }
        public string updateEmail(MessageFromPastSelf messageFromPastSelf)
        {
            BackgroundJob.Delete(messageFromPastSelf.jobid);
            return BackgroundJob.Schedule(
                () => sendEmail(messageFromPastSelf),
                TimeSpan.FromSeconds(messageFromPastSelf.when));
        }
        public bool deleteEmail(MessageFromPastSelf messageFromPastSelf)
        {
           return BackgroundJob.Delete(messageFromPastSelf.jobid);

        }
        public void sendEmail(MessageFromPastSelf messageFromPastSelf)
        {
            using (var message = new MailMessage())
            {
                Console.WriteLine(this._email);
                Console.WriteLine(this._password);
                message.To.Add(new MailAddress(messageFromPastSelf.To));
                message.From = new MailAddress(this._email);
                message.Subject = messageFromPastSelf.Subject;
                message.Body = messageFromPastSelf.Body;

                message.IsBodyHtml = messageFromPastSelf.IsBodyHtml;
                using (var client = new SmtpClient("smtp.gmail.com"))
                {
                    client.Port = 587;
                    client.Credentials = new NetworkCredential(_email, _password);
                    client.EnableSsl = true;
                    client.Send(message);
                }
            }
        }
    }
}