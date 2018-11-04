using System;
using System.Net;
using System.Net.Mail;

namespace EBFP.BL.Helper
{
    public class MailHelper
    {
        public string EmailTo { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public bool SendEmail()
        {
            var mail_SMTP = "smtp.gmail.com";
            var mail_SMTP_PORT = "587";
            var mail_USER = "ebfp2018@gmail.com";
            var mail_PASSWORD = "Admin1@3";
            using (var mail = new MailMessage())
            {
                try
                {
                    var MAIL_SMTP = mail_SMTP;
                    var MAIL_SMTP_PORT = Convert.ToInt32(mail_SMTP_PORT);
                    var MAIL_USER = mail_USER;
                    var MAIL_PASSWORD = mail_PASSWORD;

                    mail.From = new MailAddress(MAIL_USER);
                    mail.To.Add(EmailTo);
                    mail.Subject = Subject;
                    mail.Body = Body;
                    mail.IsBodyHtml = true;

                    using (var smtp = new SmtpClient(MAIL_SMTP, MAIL_SMTP_PORT))
                    {
                        smtp.ServicePoint.MaxIdleTime = 1;
                        smtp.Credentials = new NetworkCredential(MAIL_USER, MAIL_PASSWORD);
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                    return true;
                }
                    // ReSharper disable once RedundantCatchClause
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

    }
}