using TrackerService.Data.Contracts;
using TrackerService.Data.Repositories;

namespace TrackerService.Data
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly string connString;
        private readonly StorageConnectionInfo storageConnInfo;

        public RepositoryFactory(string connString, StorageConnectionInfo storageConnInfo)
        {
            this.connString = connString;
            this.storageConnInfo = storageConnInfo;
        }

        public IEmployeeRepository CreateEmployeeRepository()
        {
            return new EmployeeSqlRepository(connString);
        }

        public IDocumentStorageRepository CreateDocumentStorageRepository()
        {
            return new BlobStorageRepository(storageConnInfo);
        }

        public ITimesheetRepository CreateTimesheetRepository()
        {
            return new TimesheetSqlRepository(connString);
        }
    }
}
