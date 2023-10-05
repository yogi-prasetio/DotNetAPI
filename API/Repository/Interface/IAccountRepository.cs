using API.ViewModel;

namespace API.Repository.Interface
{
    public interface IAccountRepository
    {
        int Login(LoginViewModel loginViewModel);
        int ForgotPassword(string email);
        int ChangePassword(PasswordViewModel passwordViewModel);
    }
}
