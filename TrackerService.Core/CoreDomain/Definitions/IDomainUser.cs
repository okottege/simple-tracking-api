namespace TrackerService.Core.CoreDomain.Definitions
{
    public interface IDomainUser
    {
        string Id { get; }
        string Email { get; }
        string FirstName { get; }
        string LastName { get; }
    }
}