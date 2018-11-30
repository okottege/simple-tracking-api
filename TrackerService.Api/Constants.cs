namespace TrackerService.Api
{
    public static class UserClaimTypes
    {
        public const string USER_ID = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
        public const string EMAIL = "https://tracking-service/email";
        public const string ROLES = "http://schemas.microsoft.com/ws/2008/06/identity/claims/roles";
    }
}
