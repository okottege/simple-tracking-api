using AutoMapper;
using TrackerService.Api.CoreDomain.Tasks;
using TrackerService.Api.ViewModels.Tasks;
using TrackerService.Core.CoreDomain;
using TrackerService.Core.CoreDomain.Tasks;
using TrackerService.Core.CoreDomain.Tasks.Definitions;

namespace TrackerService.Api.MappingProfiles.Tasks
{
    public class TaskAssignmentProfile : Profile
    {
        public TaskAssignmentProfile()
        {
            CreateMap<AssignmentViewModel, Assignment>()
                .ForMember(m => m.Type, opt => opt.MapFrom(src => EnumerationExtensions.FromString<AssignmentEntityType>(src.Type)));
            CreateMap<ITaskAssignment, AssignmentViewModel>()
                .ForMember(m => m.Type, opt => opt.MapFrom(src => src.Type.ToString()));
        }
    }
}
