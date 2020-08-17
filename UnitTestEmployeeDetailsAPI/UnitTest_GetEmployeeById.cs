using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Web.Http;
using EmployeeDetailsAPI.Controllers;
using EmployeeDetailsAPI.Models;

namespace UnitTestEmployeeDetailsAPI
{
    [TestClass]
    public class UnitTest_GetEmployeeById
    {
        [TestMethod]
        public void EmployeeGetById_Invalid()
        {
            var Controller = new EmployeeController();
            Controller.Request = new HttpRequestMessage();
            Controller.Configuration = new HttpConfiguration();  
            var response = Controller.Get("EN_0002"); 
            Employee employee;
            Assert.IsTrue(response.TryGetContentValue<Employee>(out employee));
            Assert.AreEqual("Musunur", employee.FullName);
        }

        [TestMethod]
        public void EmployeeGetById_valid()
        {
            var Controller = new EmployeeController();
            Controller.Request = new HttpRequestMessage();
            Controller.Configuration = new HttpConfiguration(); 
            var response = Controller.Get("EN_0002"); 
            Employee employee;
            Assert.IsTrue(response.TryGetContentValue<Employee>(out employee));
            Assert.AreEqual("Advitiya Sujeet", employee.FullName.Trim());
        }
    }
}
