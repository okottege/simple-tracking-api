using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TrackerService.Api.ViewModels;
using TrackerService.Data.Contracts;
using TrackerService.Data.DataObjects;

namespace TrackerService.Api.Controllers
{
    [ApiController]
    [Route("api/employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository repository;

        public EmployeeController(IRepositoryFactory factory)
        {
            repository = factory.CreateEmployeeRepository();
        }

        [ProducesResponseType(200, Type = typeof(EmployeeViewModel))]
        [ProducesResponseType(404)]
        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetEmployee(int employeeId)
        {
            var employee = await repository.GetEmployee(employeeId);
            if (employee != null)
            {
                return Ok(new EmployeeViewModel
                {
                    EmployeeId = employee.EmployeeId,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    DateOfBirth = employee.DateOfBirth,
                    StartDate = employee.StartDate
                });
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] EmployeeViewModel employee)
        {
            var newEmployee = await repository.Create(new Employee
            {
                EmployeeId = employee.EmployeeId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                DateOfBirth = employee.DateOfBirth.Value,
                StartDate = employee.StartDate.Value
            });

            return CreatedAtAction(nameof(GetEmployee), new {employeeId = newEmployee.EmployeeId}, newEmployee);
        }

        [HttpDelete("{employeeId}")]
        public async Task<IActionResult> Remove(int employeeId)
        {
            await repository.Remove(employeeId);
            return NoContent();
        }
    }
}
