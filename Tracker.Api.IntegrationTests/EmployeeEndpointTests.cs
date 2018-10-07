using System;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Tracker.Api.IntegrationTests
{
    [TestClass]
    public class EmployeeEndpointTests : BaseIntegrationTest
    {
        [ClassInitialize]
        public static async Task InitialiseTests(TestContext context)
        {
            await Initialise(context);
        }

        [TestMethod]
        public async Task TestEmployeeCRUDOperations()
        {
            dynamic employee = new ExpandoObject();
            employee.StartDate = new DateTime(2004, 11, 15);
            employee.DateOfBirth = new DateTime(2000, 12, 3);
            employee.LastName = "Smith";
            employee.FirstName = "Bob";

            int employeeId = await TestEmployeeCreate(employee);
            employee.EmployeeId = employeeId;

            await TestGetEmployeeById(employee);
            await TestDeleteEmployee(employee);
        }

        [TestMethod]
        public async Task TestCreateEmployeeWithValidationErrors()
        {
            dynamic employee = new ExpandoObject();
            var reqContent = new StringContent(JsonConvert.SerializeObject(employee));
            reqContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await serviceClient.PostAsync("api/employee", reqContent);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            dynamic data = await GetResponsePayload(response);
            string firstNameError = JArray.FromObject(data.FirstName)[0].ToString();
            string lastNameError = JArray.FromObject(data.LastName)[0].ToString();
            string dateOfBirthError = JArray.FromObject(data.DateOfBirth)[0].ToString();
            string startDateError = JArray.FromObject(data.StartDate)[0].ToString();

            new[] {firstNameError, lastNameError, dateOfBirthError, startDateError}.ToList().ForEach(e => Assert.IsTrue(e.EndsWith("field is required.")));
        }

        private async Task<int> TestEmployeeCreate(dynamic employee)
        {
            logger.WriteLine("Testing creation of a new employee");
            
            var reqContent = new StringContent(JsonConvert.SerializeObject(employee));
            reqContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await serviceClient.PostAsync("api/employee", reqContent);
            response.EnsureSuccessStatusCode();
            dynamic data = await GetResponsePayload(response);

            CheckEmployeeInfoWithPayload(employee, data);
            
            int employeeId = Convert.ToInt32(data.EmployeeId.ToString());
            Assert.IsTrue(employeeId > 0);
            Assert.IsTrue(response.Headers.Location.ToString().EndsWith($"api/employee/{employeeId}"));

            logger.WriteLine("Creation of new employee test was successful.");
            return employeeId;
        }

        private async Task TestGetEmployeeById(dynamic employee)
        {
            logger.WriteLine($"Test getting employee by Id: {employee.EmployeeId}");

            var response = await serviceClient.GetAsync($"api/employee/{employee.EmployeeId}");
            response.EnsureSuccessStatusCode();
            var data = await GetResponsePayload(response);

            CheckEmployeeInfoWithPayload(employee, data);
            logger.WriteLine("Getting employee by id was successful.");
        }

        private async Task TestDeleteEmployee(dynamic employee)
        {
            logger.WriteLine($"Test deleting employee by Id: {employee.EmployeeId}");

            var response = await serviceClient.DeleteAsync($"api/employee/{employee.EmployeeId}");
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

            var respGetRemovedEmployee = await serviceClient.GetAsync($"api/employee/{employee.EmployeeId}");
            Assert.AreEqual(HttpStatusCode.NotFound, respGetRemovedEmployee.StatusCode);

            logger.WriteLine("Deleting employee by id was successful.");
        }

        private void CheckEmployeeInfoWithPayload(dynamic employee, dynamic payload)
        {
            Assert.AreEqual(employee.FirstName.ToString(), payload.FirstName.ToString());
            Assert.AreEqual(employee.LastName.ToString(), payload.LastName.ToString());
            Assert.AreEqual(employee.DateOfBirth.ToString(), payload.DateOfBirth.ToString());
            Assert.AreEqual(employee.StartDate.ToString(), payload.StartDate.ToString());
            Assert.IsNotNull(payload.EmployeeId);
        }
    }
}
