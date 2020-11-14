using Mango.Core.ControllerAbstractions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Service.OpenSource.Api.Controllers
{
    /// <summary>
    /// 健康检查controller
    /// </summary>
    [Route("api/opensource/")]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet("healthcheck")]
        public IActionResult Check()
        {
            return Ok();
        }
    }
}
