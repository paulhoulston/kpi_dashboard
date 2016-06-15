using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Optionis.KPIs.Dashboard.Application
{
    [DefaultValue(Unknown)]
    public enum DeploymentStatus
    {
        Unknown = 0,
        Pending = 1,
        Success = 2,
        Failed = 3,
        Aborted = 4
    }
    
}
