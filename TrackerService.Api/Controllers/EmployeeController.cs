using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper mapper;

        public EmployeeController(IRepositoryFactory factory, IMapper mapper)
        {
            repository = factory.CreateEmployeeRepository();
            this.mapper = mapper;
        }

        [ProducesResponseType(200, Type = typeof(IEnumerable<Employee>))]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var employees = await repository.GetAllEmployees();
            return Ok(mapper.Map<List<EmployeeViewModel>>(employees));
        }

        [ProducesResponseType(200, Type = typeof(EmployeeViewModel))]
        [ProducesResponseType(404)]
        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetEmployee(int employeeId)
        {
            var employee = await repository.GetEmployee(employeeId);
            var vmEmployee = mapper.Map<EmployeeViewModel>(employee);
            return Ok(vmEmployee);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] EmployeeViewModel vmEmployee)
        {
            var employee = await repository.Create(mapper.Map<Employee>(vmEmployee));
            return CreatedAtAction(nameof(GetEmployee), new {employeeId = employee.EmployeeId}, employee);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] EmployeeViewModel employee)
        {
            await repository.Update(mapper.Map<Employee>(employee));
            return NoContent();
        }

        [HttpDelete("{employeeId}")]
        public async Task<IActionResult> Remove(int employeeId)
        {
            await repository.Remove(employeeId);
            return NoContent();
        }
    }
}
