using AutoMapper;
using TrackerService.Api.CoreDomain.Tasks;
using TrackerService.Api.ViewModels.Tasks;
using TrackerService.Core.CoreDomain;
using TrackerService.Core.CoreDomain.Tasks;
using TrackerService.Core.CoreDomain.Tasks.Definitions;

namespace TrackerService.Api.MappingProfiles.Tasks
{
    public class TaskDependencyMappingProfile : Profile
    {
        public TaskDependencyMappingProfile()
        {
            CreateMap<TaskDependencyViewModel, ITaskDependency>()
                .ForMember(m => m.Type, opt => opt.MapFrom(src => EnumerationExtensions.FromString<DependencyType>(src.Type)))
                .As<TaskDependency>();
            CreateMap<ITaskDependency, TaskDependencyViewModel>()
                .ForMember(m => m.Type, opt => opt.MapFrom(src => src.Type.ToString()));
        }
    }
}
