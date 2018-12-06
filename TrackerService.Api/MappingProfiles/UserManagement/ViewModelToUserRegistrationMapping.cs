using AutoMapper;
using TrackerService.Api.ViewModels.UserManagement;
using TrackerService.Data.DataObjects;

namespace TrackerService.Api.MappingProfiles.UserManagement
{
    public class ViewModelToUserRegistrationMapping : Profile
    {
        public ViewModelToUserRegistrationMapping()
        {
            CreateMap<RegistrationViewModel, UserRegistration>();
        }
    }
}
