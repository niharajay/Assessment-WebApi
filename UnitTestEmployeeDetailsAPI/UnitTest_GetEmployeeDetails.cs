using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Web.Http;
using EmployeeDetailsAPI.Controllers;
using EmployeeDetailsAPI.Models;
using System.Collections.Generic;

namespace UnitTestEmployeeDetailsAPI
{
    [TestClass]
    public class UnitTest_GetEmployeeDetails
    {
        [TestMethod]
        public void GetEmployeeDetails_Invalid()
        {
            var Controller = new EmployeeController();
            Controller.Request = new HttpRequestMessage();
            Controller.Configuration = new HttpConfiguration();  
            var response = Controller.GetEmployeeDetails("Ad*",String.Empty,"Sri Lanka","Colombo");  
            List <Employee> empList;
            Assert.IsTrue(response.TryGetContentValue<List<Employee>>(out empList));
            Assert.IsTrue(empList.Count > 0);
            Assert.AreEqual("Musunur", empList[0].FullName);
        }

        [TestMethod]
        public void GetEmployeeDetail_valid()
        {
            var Controller = new EmployeeController();
            Controller.Request = new HttpRequestMessage();
            Controller.Configuration = new HttpConfiguration(); 
            var response = Controller.GetEmployeeDetails("Ad*", String.Empty, "Sri Lanka", "Trinco"); 
            List<Employee> empList;
            Assert.IsTrue(response.TryGetContentValue<List<Employee>>(out empList));
            Assert.IsTrue(empList.Count > 0);
            Assert.AreEqual("Advitiya Sujeet", empList[0].FullName.Trim());
        }
    }
}
