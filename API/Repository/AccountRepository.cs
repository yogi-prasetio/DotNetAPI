using API.context;
using API.Repository.Interface;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.SqlServer.Server;
using MimeKit;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;
using System.Numerics;
using System.Security.Authentication;
using Microsoft.EntityFrameworkCore;
using static System.Net.WebRequestMethods;
using API.Models;
using BCrypt.Net;
using API.ViewModel;

namespace API.Repository
{
    public class AccountRepository: IAccountRepository
    {
        private readonly MyContext context;
        private readonly IConfiguration _config;
        private readonly int salt = 12;

        public AccountRepository(MyContext context, IConfiguration config)
        {
            this.context = context;
            _config = config;
        }

        public int ChangePassword(PasswordViewModel body)
        {
            var data = context.Employees.Join(
                                           context.Accounts,
                                           emp => emp.NIK,
                                           acc => acc.NIK,
                                           (emp, acc) => new
                                           {
                                               NIK = emp.NIK,
                                               FullName = emp.FirstName + " " + emp.LastName,
                                               Email = emp.Email,
                                               Password = acc.Password,
                                               OTP = acc.OTP,
                                               Expired = acc.Expired
                                           }
                                       ).SingleOrDefault(e => e.Email == body.Email);
            if (data == null)
            {
                return 0;
            }
            else
            {
                DateTime now = DateTime.Now;
                if (body.Password != body.VerifyPassword)
                {
                    return 0;
                }
                else if (body.OTP != data.OTP)
                {
                    return -1;
                }
                else if (data.Expired < now)
                {
                    return -2;

                }
                else
                {
                    string hashPassword = BCrypt.Net.BCrypt.HashPassword(body.Password, salt);
                    var account = new Account
                    {
                        NIK = data.NIK,
                        Password = hashPassword,
                        OTP = data.OTP,
                    };
                    context.Entry(account).State = EntityState.Modified;
                    context.SaveChanges();

                    return 1;
                }
            }
        }

        public bool ForgotPassword(string email)
        {
            var data = context.Employees.Join(
                                        context.Accounts,
                                        emp => emp.NIK,
                                        acc => acc.NIK,
                                        (emp, acc) => new { 
                                            NIK = emp.NIK,
                                            FullName = emp.FirstName + " "+ emp.LastName,
                                            Email = emp.Email,
                                            Password = acc.Password,
                                        }
                                    ).SingleOrDefault(e => e.Email == email);
            if (data == null) { 
                return false;
            } else
            {
                var mail = new MimeMessage();
                var emailConfig = _config.GetValue<String>("EmailConfiguration");

                mail.From.Add(new MailboxAddress("DotNet App", "manghellfrog666@gmail.com"));
                mail.To.Add(new MailboxAddress(data.FullName, email));

                var bodyBuilder = new BodyBuilder();

                Random rand = new Random();
                string OTP = (rand.Next(999999)).ToString();

                var check_otp = context.Accounts.SingleOrDefault(acc => acc.OTP == OTP);

                if (check_otp != null)
                {
                    return false;
                }
                else
                {
                    DateTime date = DateTime.Now;
                    TimeSpan time = new TimeSpan(0, 0, 2, 0);
                    DateTime exp = date.Add(time);
                    var account = new Account
                    {
                        NIK = data.NIK,
                        Password = data.Password,
                        OTP = OTP,
                        Expired = exp
                    };
                    context.Entry(account).State = EntityState.Modified;
                    context.SaveChanges();

                    mail.Subject = "Reset Password";
                    bodyBuilder.HtmlBody = "Hello " + data.FullName + ", please don't give this code to anyone. Your OTP Code is <b>" + OTP + "</b>";
                    mail.Body = bodyBuilder.ToMessageBody();
                    using (var smtp = new MailKit.Net.Smtp.SmtpClient())
                    {
                        smtp.CheckCertificateRevocation = false;
                        smtp.Connect("smtp.gmail.com", 465, true);

                        // Note: only needed if the SMTP server requires authentication
                        smtp.Authenticate("manghellfrog666@gmail.com", "d e dw nlmv s ics r ero");

                        smtp.Send(mail);
                        smtp.Disconnect(true);
                    }
                    return true;
                }
            }
        }

        public bool Login(string email, string password)
        {
            var emp = context.Employees.Join(
                                        context.Accounts,
                                        emp => emp.NIK,
                                        acc => acc.NIK,
                                        (emp, acc) => new { 
                                            Email = emp.Email,
                                            Password = acc.Password
                                        }
                                    ).Where(e => e.Email == email).FirstOrDefault();
            bool verify = BCrypt.Net.BCrypt.Verify(password, emp.Password);

            return emp != null && verify !=false;
        }
    }
}
