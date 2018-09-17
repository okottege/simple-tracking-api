using TrackerService.Data.Contracts;
using TrackerService.Data.Repositories;

namespace TrackerService.Data
{
    public static class RepositoryFactory
    {
        public static IEmployeeRepository CreateEmployeeRepository(string connString)
        {
            return new EmployeeSqlRepository(connString);
        }
    }
}
