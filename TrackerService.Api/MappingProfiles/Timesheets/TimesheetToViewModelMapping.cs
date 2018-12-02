using AutoMapper;
using TrackerService.Api.ViewModels;
using TrackerService.Data.DataObjects;

namespace TrackerService.Api.MappingProfiles.Timesheets
{
    public class TimesheetToViewModelMapping : Profile
    {
        public TimesheetToViewModelMapping()
        {
            CreateMap<Timesheet, TimesheetViewModel>();
            CreateMap<TimesheetEntry, TimesheetEntryViewModel>();
        }
    }
}
