using API.Models;

namespace API.ViewModel
{
    public class RegisterViewModel
    {
        //public Employee employee{ get; set; }
        //public Account account{ get; set; }
        //public Profiling profiling{ get; set; }
        //public Education education { get; set; }
        //public University university{ get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }
        public int Salary { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public string Password { get; set; }
        public Degree Degree { get; set; }
        public float GPA { get; set; }
        public int University_Id { get; set; }
    }
}
