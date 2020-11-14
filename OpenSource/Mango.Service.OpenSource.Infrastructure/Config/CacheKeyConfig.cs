using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Service.OpenSource.Infrastructure.Config
{
    public class CacheKeyConfig
    {
        private const string _prefix = "service:opensource:";

        public const string ProjectDetail = _prefix + "detail";

        public const string ProjectList = _prefix + "list";
    }
}
