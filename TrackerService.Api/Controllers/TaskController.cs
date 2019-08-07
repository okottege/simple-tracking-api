using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackerService.Api.ViewModels.Tasks;
using TrackerService.Core.CoreDomain.Tasks;
using TrackerService.Core.Repositories;
using TrackerService.Data.Contracts;

namespace TrackerService.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/tasks")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskRepository taskRepo;
        private readonly IMapper mapper;

        public TaskController(IRepositoryFactory repoFactory, IMapper mapper)
        {
            taskRepo = repoFactory.CreateTaskRepository();
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([Required] CreateTaskViewModel model)
        {
            var task = mapper.Map<PlatformTask>(model);
            var taskId = await taskRepo.CreateNewTask(task);
            return Ok(new {taskId});
        }

        [HttpGet("{taskId}")]
        public async Task<IActionResult> RetrieveTask(string taskId)
        {
            var task = await taskRepo.GetTask(taskId);
            var model = mapper.Map<TaskDetailsViewModel>(task);
            return Ok(model);
        }
    }
}
