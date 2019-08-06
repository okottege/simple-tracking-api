using AutoMapper;
using TrackerService.Api.ViewModels.Tasks;
using TrackerService.Core.CoreDomain.Tasks;
using TrackerService.Core.CoreDomain.Tasks.Definitions;

namespace TrackerService.Api.MappingProfiles.Tasks
{
    public class CreateTaskMappingProfile : Profile
    {
        public CreateTaskMappingProfile()
        {
            CreateMap<CreateTaskViewModel, PlatformTask>()
                .ForMember(dest => dest.Parent,
                    opt => opt.MapFrom(src => new PlatformTask { TaskId = src.ParentTaskId }));
        }
    }
}
