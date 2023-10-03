using API.ViewModel;

namespace API.Repository.Interface
{
    public interface IAccountRepository
    {
        bool Login(string email, string password);
        bool ForgotPassword(string email);
        int ChangePassword(PasswordViewModel passwordViewModel);
    }
}
