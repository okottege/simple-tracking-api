using AutoMapper;
using TrackerService.Api.CoreDomain.Tasks;
using TrackerService.Api.ViewModels.Tasks;

namespace TrackerService.Api.MappingProfiles.Tasks
{
    public class TaskSearchMappingProfile : Profile
    {
        public TaskSearchMappingProfile()
        {
            CreateMap<TaskSearchViewModel, TaskFilterOptions>();
        }
    }
}
