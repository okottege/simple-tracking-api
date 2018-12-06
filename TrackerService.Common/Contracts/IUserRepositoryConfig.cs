namespace TrackerService.Common.Contracts
{
    public interface IUserRepositoryConfig
    {
        string ConnectionName { get; }
        string UserManagementBaseUrl { get; }
    }
}
