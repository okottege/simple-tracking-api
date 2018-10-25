using System.Collections.Generic;
using System.Threading.Tasks;
using TrackerService.Data.DataObjects;

namespace TrackerService.Data.Contracts
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllEmployees();

        Task<Employee> GetEmployee(int id);

        Task<Employee> Create(Employee employee);

        Task<bool> Update(Employee employee);

        Task<bool> Remove(int employeeId);
    }
}