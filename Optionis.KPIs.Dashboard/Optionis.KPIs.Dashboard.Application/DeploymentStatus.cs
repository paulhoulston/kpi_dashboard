using System.ComponentModel;

namespace Optionis.KPIs.Dashboard.Application
{
    [DefaultValue(Pending)]
    public enum DeploymentStatus
    {
        Pending = 0,
        Success = 1,
        Failed = 2,
        Aborted = 3
    }
    
}
