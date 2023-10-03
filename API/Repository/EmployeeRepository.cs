using API.context; 
using API.Models;
using API.Repository.Interface;
using API.ViewModel;
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Numerics;

namespace API.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly MyContext context;
        public EmployeeRepository(MyContext context)
        {
            this.context = context;
        }
        public IEnumerable<Employee> Get()
        {
            return context.Employees.ToList();
        }
        public Employee Get(string NIK)
        {
            var entity = context.Employees.Find(NIK);
            return entity;
        }
        public int Insert(Employee employee)
        {
            employee.NIK = GenerateNIK();
            context.Employees.Add(employee);
            var result = context.SaveChanges();
            return result;
        }
        public int Update(Employee employee)
        {
            context.Entry(employee).State = EntityState.Modified;
            var result = context.SaveChanges();
            return result;
        }
        public int Delete(string NIK)
        {
            var entity = context.Employees.Find(NIK);
            context.Remove(entity);
            var result = context.SaveChanges();
            return result;
        }

        public int FindPhone(string phone)
        {
            var row = context.Employees.Where(e => e.Phone == phone).ToList();
            return row.Count;
        }

        public int FindEmail(string email)
        {
            var row = context.Employees.Where(e => e.Email == email).ToList();
            return row.Count;
        }
        public string GenerateNIK()
        {
            string date = DateTime.Now.ToString("ddMMyy");
            var last = context.Employees.OrderByDescending(e => e.NIK).FirstOrDefault();
            if (last == null)
            {
                return date + "001";
            }
            else
            {
                string lastId = last.NIK.Substring(last.NIK.Length - 3);
                int numberId = int.Parse(lastId) + 1;
                return date + numberId.ToString("000");
            }
        }

        public int Register(RegisterViewModel model)
        {
            Employee emp = new Employee();
            Account acc = new Account();
            Profiling prof = new Profiling();
            Education edu = new Education();
            University univ = new University();

            var check_univ = context.Universities.Find(model.University_Id);

            if (check_univ == null)
            {
                return -1;
            }
            else
            {
                //Employee Value
                emp.NIK = GenerateNIK();
                emp.FirstName = model.FirstName;
                emp.LastName = model.LastName;
                emp.Phone = model.Phone;
                emp.BirthDate = model.BirthDate;
                emp.Salary = model.Salary;
                emp.Email = model.Email;
                emp.Gender = (Gender?)model.Gender;
                context.Employees.Add(emp);
                var result_emp = context.SaveChanges();

                //Account Value
                int salt = 12;
                acc.NIK = emp.NIK;
                acc.Password = BCrypt.Net.BCrypt.HashPassword(model.Password, salt);
                context.Accounts.Add(acc);
                var result_acc = context.SaveChanges();

                //Education Value
                edu.Degree = (Degree)model.Degree;
                edu.GPA = model.GPA;
                edu.University_Id = model.University_Id;
                context.Educations.Add(edu);
                var result_edu = context.SaveChanges();

                prof.Education_Id = edu.Id;
                prof.NIK = emp.NIK;
                context.Profilings.Add(prof);
                var result_prof = context.SaveChanges();
                return result_prof;
            }
        }

        public IEnumerable<EmployeeViewModel> GetEmployeeData()
        {            
            var data = context.Employees.Join(
                                        context.Accounts,
                                        emp => emp.NIK,
                                        acc => acc.NIK,
                                        (emp, acc) => new { emp, acc }
                                    ).Join(
                                        context.Profilings,
                                        empAcc => empAcc.acc.NIK,
                                        prof => prof.NIK,
                                        (empAcc, prof) => new { empAcc, prof }
                                    ).Join(
                                        context.Educations,
                                        profEdu => profEdu.prof.Education_Id,
                                        edu => edu.Id,
                                        (profEdu, edu) => new { profEdu, edu }
                                    ).Join(
                                        context.Universities,
                                        edUniv => edUniv.edu.University_Id,
                                        univ => univ.Id,
                                        (edUniv, univ) => new EmployeeViewModel {
                                            FullName = edUniv.profEdu.empAcc.emp.FirstName + " " + edUniv.profEdu.empAcc.emp.LastName,
                                            Phone = edUniv.profEdu.empAcc.emp.Phone,
                                            BirthDate = (DateTime)edUniv.profEdu.empAcc.emp.BirthDate,
                                            Salary = (int)edUniv.profEdu.empAcc.emp.Salary,
                                            Email = edUniv.profEdu.empAcc.emp.Email,
                                            Gender = (Gender)edUniv.profEdu.empAcc.emp.Gender,
                                            Degree = edUniv.edu.Degree,
                                            GPA = edUniv.edu.GPA,
                                            UniversityName = univ.Name
                                        }
                                    ).ToList();
            return data;
        }

        public EmployeeViewModel GetEmployee(string NIK)
        {
            var data = context.Employees.Join(
                                        context.Accounts,
                                        emp => emp.NIK,
                                        acc => acc.NIK,
                                        (emp, acc) => new { emp, acc }
                                    ).Join(
                                        context.Profilings,
                                        empAcc => empAcc.acc.NIK,
                                        prof => prof.NIK,
                                        (empAcc, prof) => new { empAcc, prof }
                                    ).Join(
                                        context.Educations,
                                        profEdu => profEdu.prof.Education_Id,
                                        edu => edu.Id,
                                        (profEdu, edu) => new { profEdu, edu }
                                    ).Join(
                                        context.Universities,
                                        edUniv => edUniv.edu.University_Id,
                                        univ => univ.Id,
                                        (edUniv, univ) => new { 
                                            NIK = edUniv.profEdu.empAcc.emp.NIK,
                                            FullName = edUniv.profEdu.empAcc.emp.FirstName + " " + edUniv.profEdu.empAcc.emp.LastName,
                                            Phone = edUniv.profEdu.empAcc.emp.Phone,
                                            BirthDate = (DateTime)edUniv.profEdu.empAcc.emp.BirthDate,
                                            Salary = (int)edUniv.profEdu.empAcc.emp.Salary,
                                            Email = edUniv.profEdu.empAcc.emp.Email,
                                            Gender = (Gender)edUniv.profEdu.empAcc.emp.Gender,
                                            Degree = edUniv.edu.Degree,
                                            GPA = edUniv.edu.GPA,
                                            UniversityName = univ.Name
                                        }
                                    ).Where(e => e.NIK == NIK)
                                     .Select(r => new EmployeeViewModel
                                     {
                                         FullName = r.FullName,
                                         Phone = r.Phone,
                                         BirthDate = r.BirthDate,
                                         Salary = r.Salary,
                                         Email = r.Email,
                                         Gender = r.Gender,
                                         Degree = r.Degree,
                                         GPA = r.GPA,
                                         UniversityName = r.UniversityName
                                     }).FirstOrDefault();
            return data;
        }

    }
}
