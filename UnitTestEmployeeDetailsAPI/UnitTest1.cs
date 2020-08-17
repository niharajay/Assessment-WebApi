using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Web.Http;
using EmployeeDetailsAPI.Controllers;
using EmployeeDetailsAPI.Models;

namespace UnitTestEmployeeDetailsAPI
{
    [TestClass]
    public class UnitTestEmployeeDetails
    {
        [TestMethod]
        public void EmployeeGetById()
        {
            var Controller = new EmployeeController();
            Controller.Request = new HttpRequestMessage();
            Controller.Configuration = new HttpConfiguration();
            // Act on Test  
            var response = Controller.Get("EN_0001");
            // Assert the result  
            Employee employee;
            Assert.IsTrue(response.TryGetContentValue<Employee>(out employee));
            Assert.AreEqual("EN_0001", employee.EmployeeId);
        }
    }
}
