using System;

namespace TrackerService.Data.DataObjects
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime StartDate { get; set; }

        public string Email { get; set; }
    }
}
