using System;
using Nancy;
using Optionis.KPIs.Common;
using Optionis.KPIs.Dashboard.Application;
using System.Linq;

namespace Optionis.KPIs.Dashboard
{
    public class GetDeploymentStatuses : NancyModule
    {
        public GetDeploymentStatuses ()
        {
            Get [Routing.DeploymentStatuses.ROUTE] = _ => new {
                self = Routing.DeploymentStatuses.ROUTE,
                statuses = Enum.GetNames (typeof(DeploymentStatus)).OrderBy (status => status)
            };
        }
    }
}

