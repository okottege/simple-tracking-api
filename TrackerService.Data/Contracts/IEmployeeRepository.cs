using System.Collections.Generic;
using System.Threading.Tasks;
using TrackerService.Data.DataObjects;

namespace TrackerService.Data.Contracts
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllEmployees();
    }
}