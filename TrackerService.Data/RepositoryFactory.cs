using TrackerService.Data.Contracts;
using TrackerService.Data.Repositories;

namespace TrackerService.Data
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly string connString;

        public RepositoryFactory(string connString)
        {
            this.connString = connString;
        }

        public IEmployeeRepository CreateEmployeeRepository()
        {
            return new EmployeeSqlRepository(connString);
        }
    }
}
