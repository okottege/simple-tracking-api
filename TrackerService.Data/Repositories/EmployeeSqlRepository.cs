using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackerService.Data.Contracts;
using TrackerService.Data.DataObjects;

namespace TrackerService.Data.Repositories
{
    internal class EmployeeSqlRepository : BaseSqlRepository, IEmployeeRepository
    {
        internal EmployeeSqlRepository(string connString) : base(connString)
        {
            
        }

        public async Task<IEnumerable<Employee>> GetAllEmployees()
        {
            var result = await QueryAsync<Employee>("SELECT * FROM employee", null);
            return result;
        }

        public async Task<Employee> GetEmployee(int id)
        {
            const string SQL = "SELECT * FROM Employee WHERE EmployeeId = @id";
            var result = (await QueryAsync<Employee>(SQL, new {id})).ToList();

            return result.Any() ? result[0] : null;
        }
        
        public async Task<Employee> Create(Employee employee)
        {
            const string SQL = @"INSERT INTO [Employee] (firstName, lastName, dateOfBirth, startDate) 
                               VALUES (@firstName, @lastName, @dateOfBirth, @startDate);
                               SELECT CAST(SCOPE_IDENTITY() as int)";
            var id = await Insert(SQL,
                new
                {
                    firstName = employee.FirstName,
                    lastName = employee.LastName,
                    dateOfBirth = employee.DateOfBirth,
                    startDate = employee.StartDate
                });
            employee.EmployeeId = id;
            return employee;
        }

        public async Task<bool> Remove(int employeeId)
        {
            return await Delete("DELETE FROM Employee WHERE employeeId = @employeeId", new {employeeId});
        }
    }
}
