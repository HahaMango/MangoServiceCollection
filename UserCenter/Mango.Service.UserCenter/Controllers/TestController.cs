using Mango.Core.Authentication.Jwt;
using Mango.Core.ControllerAbstractions;

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