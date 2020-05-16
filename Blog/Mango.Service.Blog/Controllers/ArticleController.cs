using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mango.Core.ApiResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mango.Core.Enums;

namespace Mango.Service.Blog.Controllers
{
    [ApiController]
    public class ArticleController : ControllerBase
    {
        [HttpGet("api/article/test")]
        public async Task<ApiResult<string>> Test()
        {
            return await Task.FromResult(new ApiResult<string>
            {
                Code = Code.OK,
                Message = "查询成功",
                Data = "data"
            });
        }
    }
}