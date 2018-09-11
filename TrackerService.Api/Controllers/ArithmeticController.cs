using Microsoft.AspNetCore.Mvc;

namespace TrackerService.Api.Controllers
{
    [Route("api/arithmetic")]
    public class ArithmeticController : ControllerBase
    {
        [Route("add/{a}/with/{b}")]
        public ActionResult<decimal> Add(decimal a, decimal b)
        {
            return a + b;
        }
    }
}