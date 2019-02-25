namespace TrackerService.Common.Contracts
{
    public interface IBaseAuthenticationOptions
    {
        string Audience { get; set; }
        string ClientID { get; set; }
        string ClientSecret { get; set; }
        string GrantType { get; set; }
    }
}