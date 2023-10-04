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
using Microsoft.Extensions.Options;
using API.Utils;

namespace API.Repository
{
    public class AccountRepository: IAccountRepository
    {
        private readonly MyContext context;
        private readonly EmailConfig _config;
        private readonly int salt = 12;

        public AccountRepository(MyContext context, IOptions<EmailConfig> config)
        {
            this.context = context;
            _config = config.Value;
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

        public bool ForgotPassword(string email)
        {
            var data = context.Employees.Join(
                                        context.Accounts,
                                        emp => emp.NIK,
                                        acc => acc.NIK,
                                        (emp, acc) => new {
                                            NIK = emp.NIK,
                                            FullName = emp.FirstName + " " + emp.LastName,
                                            Email = emp.Email,
                                            Password = acc.Password,
                                        }
                                    ).SingleOrDefault(e => e.Email == email);
            if (data == null)
            {
                return false;
            }
            else
            {
                string OTP = GenerateOTP();
                if (CheckOTP(OTP) == false)
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

                    string msg = "Hello " + data.FullName + ", please don't give this code to anyone. Your OTP Code is <b>" + OTP + "</b>";

                    IOptions<EmailConfig> config;
                    EmailHelpers mail = new EmailHelpers(_config);
                    mail.SendMail(data.FullName, data.Email, "Reset Password", msg);

                    return true;
                }
            }
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
                    return -3;
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
        public string GenerateOTP()
        {
            Random rand = new Random();
            return (rand.Next(999999)).ToString();
        }

        public bool CheckOTP(string OTP)
        {
            var result = context.Accounts.SingleOrDefault(acc => acc.OTP == OTP);
            if (result == null) { return true; } else { return false; }
        }
    }
}
