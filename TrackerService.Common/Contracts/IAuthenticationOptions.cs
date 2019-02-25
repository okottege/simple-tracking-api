namespace TrackerService.Common.Contracts
{
    interface IAuthenticationOptions : IBaseAuthenticationOptions
    {
        string Authority { get; set; }
        string Realm { get; set; }
    }
}
