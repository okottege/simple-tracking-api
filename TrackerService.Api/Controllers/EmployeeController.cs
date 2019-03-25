using System.Collections.Generic;
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

        [ProducesResponseType(200, Type = typeof(IEnumerable<Employee>))]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var employees = await repository.GetAllEmployees();
            return Ok(employees);
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
                    StartDate = employee.StartDate,
                    Email = employee.Email
                });
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] EmployeeViewModel employee)
        {
            var newEmployee = await repository.Create(GetEmployee(employee));
            return CreatedAtAction(nameof(GetEmployee), new {employeeId = newEmployee.EmployeeId}, newEmployee);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] EmployeeViewModel employee)
        {
            var updated = await repository.Update(GetEmployee(employee));

            if (updated)
            {
                return Ok();
            }

            return NotFound();
        }

        [HttpDelete("{employeeId}")]
        public async Task<IActionResult> Remove(int employeeId)
        {
            await repository.Remove(employeeId);
            return NoContent();
        }

        private Employee GetEmployee(EmployeeViewModel employeeVM)
        {
            return new Employee
            {
                EmployeeId = employeeVM.EmployeeId,
                FirstName = employeeVM.FirstName,
                LastName = employeeVM.LastName,
                DateOfBirth = employeeVM.DateOfBirth.Value,
                StartDate = employeeVM.StartDate.Value,
                Email = employeeVM.Email
            };
        }
    }
}
