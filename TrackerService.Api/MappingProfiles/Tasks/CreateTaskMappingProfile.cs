using AutoMapper;
using TrackerService.Api.ViewModels.Tasks;
using TrackerService.Core.CoreDomain.Tasks;

namespace TrackerService.Api.MappingProfiles.Tasks
{
    public class CreateTaskMappingProfile : Profile
    {
        public CreateTaskMappingProfile()
        {
            CreateMap<CreateTaskViewModel, ITask>();
        }
    }
}
