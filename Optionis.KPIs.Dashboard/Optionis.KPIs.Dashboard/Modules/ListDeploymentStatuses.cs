﻿using System;
using Nancy;
using Optionis.KPIs.Dashboard.Application;
using System.Linq;
using Optionis.KPIs.Dashboard.Modules.Routes;

namespace Optionis.KPIs.Dashboard
{
    public class ListDeploymentStatuses : NancyModule
    {
        public ListDeploymentStatuses ()
        {
            Get [Routing.DeploymentStatuses.ROUTE] = _ => new {
                links = new {
                    self = Routing.DeploymentStatuses.ROUTE
                },
                statuses = Enum.GetNames (typeof(DeploymentStatus))
                    .Select (status => new { status })
                    .OrderBy (status => status.status)
                };
        }
    }
}

