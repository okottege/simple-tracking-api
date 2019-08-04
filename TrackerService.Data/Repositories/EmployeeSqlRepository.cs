using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackerService.Common.Exceptions;
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

            if (!result.Any()) EmployeeNotFound(id);

            return  result[0];
        }
        
        public async Task<Employee> Create(Employee employee)
        {
            const string SQL = @"INSERT INTO [Employee] (firstName, lastName, dateOfBirth, startDate, email) 
                               VALUES (@firstName, @lastName, @dateOfBirth, @startDate, @email);
                               SELECT CAST(SCOPE_IDENTITY() as int)";
            
            var id = await Insert<int>(SQL,
                new
                {
                    firstName = employee.FirstName,
                    lastName = employee.LastName,
                    dateOfBirth = employee.DateOfBirth,
                    startDate = employee.StartDate,
                    email = employee.Email
                });
            employee.EmployeeId = id;
            return employee;
        }

        public async Task<bool> Update(Employee employee)
        {
            const string SQL = @"UPDATE [Employee] 
                                SET firstName = @firstName, lastName = @lastName, dateOfBirth = @dateOfBirth, startDate = @startDate
                                WHERE employeeId = @employeeId";
            var args = new
            {
                employeeId = employee.EmployeeId,
                firstName = employee.FirstName,
                lastName = employee.LastName,
                dateOfBirth = employee.DateOfBirth,
                startDate = employee.StartDate
            };
            var updated = await ExecuteCommand(SQL, args);
            if(!updated) EmployeeNotFound(employee.EmployeeId);

            return true;
        }

        public async Task<bool> Remove(int employeeId)
        {
            return await ExecuteCommand("DELETE FROM Employee WHERE employeeId = @employeeId", new {employeeId});
        }

        private void EmployeeNotFound(int id)
        {
            throw new EntityNotFoundException("Employee", id.ToString());
        }
    }
}
