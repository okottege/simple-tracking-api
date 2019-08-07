using AutoMapper;
using TrackerService.Api.CoreDomain.Tasks;
using TrackerService.Api.ViewModels.Tasks;
using TrackerService.Core.CoreDomain.Tasks.Definitions;

namespace TrackerService.Api.MappingProfiles.Tasks
{
    public class TaskContextProfile : Profile
    {
        public TaskContextProfile()
        {
            CreateMap<ContextViewModel, Context>();
            CreateMap<ContextViewModel, ITaskContextItem>().As<Context>();
            CreateMap<ITaskContextItem, ContextViewModel>();
        }
    }
}
