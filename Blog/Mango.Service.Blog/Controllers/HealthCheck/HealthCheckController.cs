using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Service.Blog.Controllers.HealthCheck
{
    [Route("api/blog/")]
    public class HealthCheckController: ControllerBase
    {
        [HttpGet("healthcheck")]
        public IActionResult Check()
        {
            return Ok();
        }
    }
}
