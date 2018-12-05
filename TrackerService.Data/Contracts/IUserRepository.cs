using TrackerService.Data.DataObjects;

namespace TrackerService.Data.Contracts
{
    public interface IUserRepository
    {
        User Register(UserRegistration registration);
    }
}
