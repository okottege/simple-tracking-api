﻿using AutoMapper;
using TrackerService.Api.ViewModels;
using TrackerService.Data.DataObjects;

namespace TrackerService.Api.MappingProfiles.Timesheets
{
    public class ViewModelToTimesheetsMapping : Profile
    {
        public ViewModelToTimesheetsMapping()
        {
            CreateMap<TimesheetViewModel, Timesheet>()
                .ForMember(dest => dest.Entries, opt => opt.MapFrom(src => src.TimesheetEntries));
            CreateMap<TimesheetEntryViewModel, TimesheetEntry>();
        }
    }
}
