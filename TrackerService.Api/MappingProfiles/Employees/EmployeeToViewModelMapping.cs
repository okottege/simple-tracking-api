using AutoMapper;
using TrackerService.Api.ViewModels;
using TrackerService.Data.DataObjects;

namespace TrackerService.Api.MappingProfiles.Employees
{
    public class EmployeeToViewModelMapping : Profile
    {
        public EmployeeToViewModelMapping()
        {
            CreateMap<Employee, EmployeeViewModel>();
            CreateMap<EmployeeViewModel, Employee>();
        }
    }
}
