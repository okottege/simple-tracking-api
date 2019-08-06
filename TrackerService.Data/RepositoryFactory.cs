using TrackerService.Core.CoreDomain;
using TrackerService.Core.Repositories;
using TrackerService.Data.Contracts;
using TrackerService.Data.Repositories;

namespace TrackerService.Data
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly string connString;
        private readonly StorageConnectionInfo storageConnInfo;
        private readonly IUserContext userContext;
        private readonly IServiceContext serviceContext;

        public RepositoryFactory(string connString, StorageConnectionInfo storageConnInfo, IServiceContext serviceContext, IUserContext userContext)
        {
            this.connString = connString;
            this.storageConnInfo = storageConnInfo;
            this.serviceContext = serviceContext;
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

        public IDBHealthCheckRepository CreateDBHealthRepository()
        {
            return new DBHealthRepository(connString);
        }

        public ITaskRepository CreateTaskRepository()
        {
            return new SqlTaskRepository(connString, serviceContext, userContext);
        }
    }
}
