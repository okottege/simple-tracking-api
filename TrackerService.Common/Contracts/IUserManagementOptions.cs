namespace TrackerService.Common.Contracts
{
    public interface IUserManagementOptions : IBaseAuthenticationOptions
    {
        string BaseUrl { get; set; }
        string ConnectionID { get; set; }
    }
}