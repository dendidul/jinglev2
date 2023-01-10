using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace Jingl.General.Utility
{
    public class EmailService
    {
        public void SendVerificationCode(string email, string password,string code, string recipient,string subject)
        {
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");


            // set smtp-client with basicAuthentication
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential(email, password);
            SmtpServer.EnableSsl = true;

            MailAddress from = new MailAddress("noreply@fameoapp.com", "FameoNotification");

            MailAddress to = new MailAddress(recipient, "FameoNotification");
            MailMessage myMail = new System.Net.Mail.MailMessage(from, to);

            // set subject and encoding
            myMail.Subject = subject;
            myMail.SubjectEncoding = System.Text.Encoding.UTF8;

            // set body-message and encoding
            myMail.Body = "Verification Code : "+code;
            myMail.BodyEncoding = System.Text.Encoding.UTF8;
            // text or html
            myMail.IsBodyHtml = true;
            //myMail.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(textHtml, new ContentType("text/html")));

            //myMail.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(textPlain, new ContentType("text/plain")));

            SmtpServer.Send(myMail);
            myMail.Dispose();
            SmtpServer.Dispose();
        }
    }
}
