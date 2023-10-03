using API.Models;

namespace API.ViewModel
{
    public class EmployeeViewModel
    {
        public string FullName { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }
        public int Salary { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public Degree Degree { get; set; }
        public float GPA { get; set; }
        public string UniversityName { get; set; }
    }
}
