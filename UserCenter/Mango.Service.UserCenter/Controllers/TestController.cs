using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mango.Core.Authentication.Jwt;
using Mango.Core.ControllerAbstractions;
using Mango.Service.UserCenter.Abstraction.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Service.UserCenter.Controllers
{
    public class TestController : MangoUserApiController
    {
        private readonly MangoJwtTokenHandler _mangoJwtTokenHandler;

        public TestController(MangoJwtTokenHandler mangoJwtTokenHandler)
        {
            _mangoJwtTokenHandler = mangoJwtTokenHandler;
        }
    }
}