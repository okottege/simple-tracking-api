using System;
using System.ComponentModel.DataAnnotations;

namespace TrackerService.Api.ViewModels
{
    public class EmployeeViewModel
    {
        public int EmployeeId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        public DateTime? StartDate { get; set; }
    }
}
