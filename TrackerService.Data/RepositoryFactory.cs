using TrackerService.Common.Contracts;
using TrackerService.Data.Contracts;
using TrackerService.Data.Repositories;

namespace TrackerService.Data
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly string connString;
        private readonly StorageConnectionInfo storageConnInfo;
        private readonly IUserContext userContext;

        public RepositoryFactory(string connString, StorageConnectionInfo storageConnInfo, IUserContext userContext)
        {
            this.connString = connString;
            this.storageConnInfo = storageConnInfo;
            this.userContext = userContext;
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
            return new TimesheetSqlRepository(connString, userContext);
        }
    }
}
