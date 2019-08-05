using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackerService.Api.ViewModels.Tasks;
using TrackerService.Core.Repositories;

namespace TrackerService.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/tasks")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskRepository taskRepo;
        private readonly IMapper mapper;

        public TaskController(ITaskRepository taskRepo, IMapper mapper)
        {
            this.taskRepo = taskRepo;
            this.mapper = mapper;
        }

        [HttpPost]
        public Task<IActionResult> CreateTask(CreateTaskViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}
