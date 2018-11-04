using System;
using System.IO;
using System.Text;
using System.Web;
using EBFP.DataAccess;

namespace EBFP.BL.Helper.Mail
{
    public class Mail : EntityFrameworkBase, IMail
    {
        private readonly string BaseUrl =
            HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);

        public Mail(EBFPEntities _context)
        {
            context_ = _context;
        }

        public bool SendConfirmDeviceEmail(string firstName, string userEmail, string confirmationKey, string empId)
        {
            var oEmail = new MailHelper
            {
                EmailTo = userEmail,
                Subject = "Bureau of Fire Protection account - Change password"
            };

            var mailBody = File.ReadAllText(HttpContext.Current.Request.PhysicalApplicationPath +
                                            @"\Content\MISC\Email_Template\Default.html");

            var sb = new StringBuilder();
            sb.Append(
                "We were asked to reset your account. Follow the instructions below if this request comes from you.<br/><br/>");
            sb.Append(
                "Ignore the E-Mail if the request to reset your password does not come from you. Don't worry, your account is safe.<br/><br/>");

            sb.Append("Click the following link to set a new password.: <a href='" + BaseUrl +
                      "/Account/ResetPassword?confirmationKey=" + confirmationKey + "&Id=" + empId +
                      "'>Change Password</a><br/><br/>");
            sb.Append(
                "If clicking the link doesn't work you can copy the link into your browser window or type it there directly.<br/>");

            mailBody = mailBody.Replace("{{Name}}", firstName)
                .Replace("{{userEmail}}", userEmail)
                .Replace("{{emailContent}}", sb.ToString());

            oEmail.Body = mailBody;
            oEmail.SendEmail();

            return true;
        }
    }

    public interface IMail
    {
        bool SendConfirmDeviceEmail(string firstName, string userEmail, string confirmationKey, string empId);
    }
}