namespace TrackerService.Data.ServiceAccessResponses
{
    public class GetTokenResponse
    {
        public string access_token { get; set; }
        public string scope { get; set; }
        public int expires_in { get; set; }
    }
}
