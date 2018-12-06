using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TrackerService.Api.Infrastructure.Filters;
using TrackerService.Api.ViewModels.UserManagement;
using TrackerService.Data.Contracts;
using TrackerService.Data.DataObjects;

namespace TrackerService.Api.Controllers
{
    [ApiController]
    [Route("api/user")]
    [ServiceFilter(typeof(RequireServiceToken))]
    public class UserManagementController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public UserManagementController(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserAsync([FromBody] RegistrationViewModel registration)
        {
            var user = await userRepository.Register(mapper.Map<UserRegistration>(registration));
            return Ok(user);
        }
    }
}
