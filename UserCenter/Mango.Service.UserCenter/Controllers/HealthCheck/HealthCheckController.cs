using Mango.Core.ControllerAbstractions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Service.UserCenter.Controllers.HealthCheck
{
    [Route("api/usercenter/")]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet("healthcheck")]
        public IActionResult Check()
        {
            return Ok();
        }
    }
}
