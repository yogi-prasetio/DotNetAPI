using API.Repository;
using API.Repository.Interface;
using API.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private AccountRepository accountRepository;

        public AccountController(AccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }
        
        [HttpPost("Login")]
        public ActionResult Login(string email, string password)
        {
            try
            {
                var emp = accountRepository.Login(email, password);
                if (emp)
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.OK, "Login Success!", emp);
                }
                else
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.NotFound, "Login Failed! Email or password is wrong.", emp);
                }
            }
            catch (Exception ex)
            {
                return ResponseHelpers.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost("ForgotPassword")]
        public ActionResult ForgotPassword(string email)
        {
            try
            {
                var send_mail = accountRepository.ForgotPassword(email);
                if (send_mail)
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.OK, "The OTP code has been sent, please check your email.");
                }
                else
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.NotFound, "Email is not registered.");
                }
            }
            catch (Exception ex)
            {
                return ResponseHelpers.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost("ChangePassword")]
        public ActionResult ChangePassword(PasswordViewModel passwordViewModel)
        {
            try
            {
                var result = accountRepository.ChangePassword(passwordViewModel);
                if (result == 1)
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.OK, "Password changed successfully.");
                }
                else if (result == 0)
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.NotImplemented, "Email is not registered!");
                }
                else if (result == -1)
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.NotImplemented, "OTP Code is wrong!");
                }
                else if (result == -2)
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.NotImplemented, "OTP Code was expired!");
                }
                else if (result == -3)
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.NotImplemented, "Doesn't match Password!");
                } else
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.NotImplemented, "There are some errors!");
                }
            }
            catch (Exception ex)
            {
                return ResponseHelpers.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
