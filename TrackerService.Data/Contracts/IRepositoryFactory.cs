﻿namespace TrackerService.Data.Contracts
{
    public interface IRepositoryFactory
    {
        IEmployeeRepository CreateEmployeeRepository();
        IDocumentStorageRepository CreateDocumentStorageRepository();
        ITimesheetRepository CreateTimesheetRepository();
        IDBHealthCheckRepository CreateDBHealthRepository();
    }
}