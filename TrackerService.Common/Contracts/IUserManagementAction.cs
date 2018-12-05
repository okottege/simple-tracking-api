namespace TrackerService.Common.Contracts
{
    /// <summary>
    /// Represents the contract for a user management action.  In this contract
    /// it contains the common data that is required such as service token in order
    /// to perform the action.
    /// </summary>
    public interface IUserManagementAction
    {
        string ServiceToken { get; set; }
    }
}
