namespace TrackerService.Data.Contracts
{
    public interface IRepositoryFactory
    {
        IEmployeeRepository CreateEmployeeRepository();
    }
}