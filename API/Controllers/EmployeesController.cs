using API.Models;
using API.Repository;
using API.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class EmployeesController : ControllerBase
    {
        private EmployeeRepository employeeRepository;

        public EmployeesController(EmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                var emp = employeeRepository.Get();
                if (emp == null)
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.NotFound, "Data is empty");
                }
                else
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.OK, "Get Employee Success", emp);
                }
            }
            catch (Exception ex)
            {
                return ResponseHelpers.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpGet("{NIK}")]
        public ActionResult Get(string NIK)
        {
            try
            {
                var emp = employeeRepository.Get(NIK);
                if (emp == null)
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.NotFound, "Employee Not Found", emp);
                }
                else
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.OK, "Get Employee Success", emp);
                }
            }
            catch (Exception ex)
            {
                return ResponseHelpers.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public ActionResult Insert(Employee employee)
        {
            try
            {
                var phone = employeeRepository.FindPhone(employee.Phone);
                var email = employeeRepository.FindEmail(employee.Email);

                if (email > 0 && phone > 0)
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.NotImplemented, "Phone and Email is registered");
                }
                else if (phone > 0)
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.NotImplemented, "Phone is registered");
                }
                else if (email > 0)
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.NotImplemented, "Email is registered");
                }
                else
                {
                    var result = employeeRepository.Insert(employee);
                    if (result > 0)
                    {
                        return ResponseHelpers.CreateResponse(HttpStatusCode.OK, "Add data successfully");
                    }
                    else
                    {
                        return ResponseHelpers.CreateResponse(HttpStatusCode.NotImplemented, "Add data failed");
                    }

                }
            }
            catch (Exception ex)
            {
                return ResponseHelpers.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpPut]
        public ActionResult Update(Employee employee)
        {
            try
            {
                var phone = employeeRepository.FindPhone(employee.Phone);
                var email = employeeRepository.FindEmail(employee.Email);                

                if (phone > 0 && email > 0)
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.NotImplemented, "Phone and Email is registered");
                }
                else if (email > 0)
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.NotImplemented, "Email is registered");
                }
                else if (phone > 0)
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.NotImplemented, "Phone is registered");
                }
                else
                {                    
                    var result = employeeRepository.Update(employee);
                    if (result > 0)
                    {
                        return ResponseHelpers.CreateResponse(HttpStatusCode.OK, "Update data successfully");
                    }
                    else
                    {
                        return ResponseHelpers.CreateResponse(HttpStatusCode.NotModified, "Update data failed");
                    }
                }
            }
            catch (Exception ex)
            {
                return ResponseHelpers.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpDelete("{NIK}")]
        public ActionResult Delete(string NIK)
        {
            try
            {
                var delete = employeeRepository.Delete(NIK);
                if (delete > 0)
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.OK, "Delete successfully");
                }
                else
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.NotFound, "Employee Not Found");
                }
            }
            catch (Exception ex)
            {
                return ResponseHelpers.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost("Register")]
        public ActionResult Register(RegisterViewModel model)
        {
            try
            {
                var phone = employeeRepository.FindPhone(model.Phone);
                var email = employeeRepository.FindEmail(model.Email);

                if (phone > 0 && email > 0)
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.NotImplemented, "Phone and Email is registered");
                }
                else if (email > 0)
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.NotImplemented, "Email is registered");
                }
                else if (phone > 0)
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.NotImplemented, "Phone is registered");
                } 
                else
                {
                    var result = employeeRepository.Register(model);
                    if (result < 0)
                    {
                        return ResponseHelpers.CreateResponse(HttpStatusCode.NotFound, "University is not found");
                    }
                    else if (result > 0)
                    {
                        return ResponseHelpers.CreateResponse(HttpStatusCode.OK, "Register successfully");
                    }
                    else
                    {
                        return ResponseHelpers.CreateResponse(HttpStatusCode.NotImplemented, "Register failed");
                    }

                }
            }
            catch (Exception ex)
            {
                return ResponseHelpers.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("Data")]
        public IActionResult GetEmployeeData()
        {
            try
            {
                var emp = employeeRepository.GetEmployeeData();
                if (emp == null)
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.NotFound, "Data is empty");
                }
                else
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.OK, "Get Employee Success", emp);
                }
            }
            catch (Exception ex)
            {
                return ResponseHelpers.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("Data/{NIK}")]
        public IActionResult GetEmployee(string NIK)
        {
            try
            {
                var emp = employeeRepository.GetEmployee(NIK);
                if (emp == null)
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.NotFound, "Data is empty");
                }
                else
                {
                    return ResponseHelpers.CreateResponse(HttpStatusCode.OK, "Get Employee Success", emp);
                }
            }
            catch (Exception ex)
            {
                return ResponseHelpers.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
