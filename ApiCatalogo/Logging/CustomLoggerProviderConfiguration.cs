using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogo.Logging
{
    public class CustomLoggerProviderConfiguration
    {
        public int EventId { get; set; }
        public LogLevel LogLevel { get; set; }
    }
}