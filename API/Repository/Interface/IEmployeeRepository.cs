using API.Models;
using API.ViewModel;

namespace API.Repository.Interface
{
    interface IEmployeeRepository
    {
        IEnumerable<Employee> Get();

        Employee Get(string NIK);
        int Insert(Employee employee);
        int Update(Employee employee);
        int Delete(string NIK);
        int FindPhone(string phone);
        int FindEmail(string email);

        int Register(RegisterViewModel viewModel);

        IEnumerable<EmployeeViewModel> GetEmployeeData();
        EmployeeViewModel GetEmployee(string NIK);
    }
}
