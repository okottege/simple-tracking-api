using AutoMapper;
using TrackerService.Api.ViewModels.Tasks;
using TrackerService.Core.CoreDomain.Tasks.Definitions;

namespace TrackerService.Api.MappingProfiles.Tasks
{
    public class TaskDetailsMappingProfile : Profile
    {
        public TaskDetailsMappingProfile()
        {
            CreateMap<ITask, TaskDetailsViewModel>();
        }
    }
}
