namespace TrackerService.Data.Contracts.UserManagement
{
    public interface IUserRepositoryConfig
    {
        string ClientId { get; }
        string ClientSecret { get; }
        string GrantType { get; }
        string ConnectionName { get; }
        string Audience { get; }
        string UserManagementBaseUrl { get; }
    }
}
