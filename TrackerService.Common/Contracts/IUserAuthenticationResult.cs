namespace TrackerService.Common.Contracts
{
    public interface IUserAuthenticationResult
    {
        string AccessToken { get; }
        int ExpiresIn { get; }
        string Scope { get; }
    }
}