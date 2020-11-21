using Mango.Core.DataStructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Service.Infrastructure.Services
{
    public interface IAuthenticationService
    {
        ControllerUser GetUser();
    }
}
