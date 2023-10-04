using API.Models;
using API.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace API.Utils
{
    public class EmailHelpers
    {
        public readonly EmailConfig _config;
        public EmailHelpers(EmailConfig config)
        {
            _config = config;
        }
        public void SendMail(string recipientName, string recipientMail, string subject, string message)
        {
            try
            {
                var mail = new MimeMessage();

                mail.From.Add(new MailboxAddress(_config.Name, _config.Username));
                mail.To.Add(new MailboxAddress(recipientName, recipientMail));

                var bodyBuilder = new BodyBuilder();
                mail.Subject = subject;
                bodyBuilder.HtmlBody = message;

                mail.Body = bodyBuilder.ToMessageBody();
                using (var smtp = new MailKit.Net.Smtp.SmtpClient())
                {
                    smtp.Connect(_config.Server, _config.Port, _config.SSL);
                    smtp.Authenticate(_config.Username, _config.Password);

                    smtp.Send(mail);
                    smtp.Disconnect(true);
                }
            }
            catch(Exception ex)
            {
                ResponseHelpers.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }        
    }
}
