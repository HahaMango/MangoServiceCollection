using Mango.Core.DataStructure;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Mango.Service.Infrastructure.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IHttpContextAccessor _httpContext;

        public AuthenticationService(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public ControllerUser GetUser()
        {
            if(_httpContext == null || _httpContext.HttpContext == null)
            {
                throw new ArgumentNullException(nameof(_httpContext));
            }
            var httpContext = _httpContext.HttpContext;
            if (httpContext.User == null)
            {
                return null;
            }
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                return null;
            }
            var identity = httpContext.User.Identity;
            var userId = httpContext.User.Claims.FirstOrDefault(item => item.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }
            var userName = httpContext.User.Claims.FirstOrDefault(item => item.Type == ClaimTypes.Name)?.Value;
            var role = httpContext.User.Claims.FirstOrDefault(item => item.Type == ClaimTypes.Role)?.Value;
            return new ControllerUser
            {
                UserId = Convert.ToInt64(userId),
                UserName = userName,
                Role = role
            };
        }
    }
}
