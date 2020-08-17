using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using EmployeeDetailsAPI.Common;
using EmployeeDetailsAPI.Models;

namespace EmployeeDetailsAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "*")]
    public class EmployeeController : ApiController
    {
        private Tools tools = new Tools();

        [HttpGet]
        [ResponseType(typeof(Employee))]
        public HttpResponseMessage Get(string EmpId)
        {
            Employee Emp = new Employee();
            try
            {
                Emp = tools.GetEmployeeDetails(EmpId);
                if(Emp == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
                throw ex;
            }
            return Request.CreateResponse(HttpStatusCode.OK, Emp);
        }

        [HttpGet]
        [ResponseType(typeof(List<Employee>))]

        public HttpResponseMessage GetEmployeeDetails(string FirstName, string LastName, string Country, string Branch)
        {
            List<Employee> EmpList = new List<Employee>();
            try
            {
                if (String.IsNullOrEmpty(FirstName) && String.IsNullOrEmpty(LastName) && String.IsNullOrEmpty(Country) && String.IsNullOrEmpty(Branch))
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }
                else
                {
                    EmpList = tools.GetFilteredEmployeeDetails(FirstName, LastName, Country, Branch);
                    if (EmpList == null || EmpList.Count == 0)
                    {
                        return new HttpResponseMessage(HttpStatusCode.BadRequest);
                    }
                }
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
                throw ex;
            }
            return Request.CreateResponse(HttpStatusCode.OK, EmpList);
        }
    }
}
