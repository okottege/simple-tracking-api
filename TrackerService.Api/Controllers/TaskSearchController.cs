using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackerService.Api.CoreDomain.Tasks;
using TrackerService.Api.ViewModels.Tasks;
using TrackerService.Core.Tasks.TaskRetrieval;

namespace TrackerService.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/tasks")]
    public class TaskSearchController : ControllerBase
    {
        private readonly ITaskRetriever taskRetriever;
        private readonly IMapper mapper;

        public TaskSearchController(ITaskRetriever taskRetriever, IMapper mapper)
        {
            this.taskRetriever = taskRetriever;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> SearchTasks([FromQuery] TaskSearchViewModel model)
        {
            var options = mapper.Map<TaskFilterOptions>(model);
            var taskList = await taskRetriever.Search(options);
            return Ok(taskList);
        }
    }
}
