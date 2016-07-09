using System;
using System.Collections.Generic;
using Nancy;
using Nancy.ModelBinding;
using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.Dashboard.Modules.Routes;
using Optionis.KPIs.DataAccess;

namespace Optionis.KPIs.Dashboard.Modules
{
    public class UpdateDeployment : NancyModule
    {
        class PatchModel
        {
            public string PropertyName { get; set; }
            public string PropertyValue { get; set; }
        }

        static readonly IDictionary<string, Func<int, string, Response>> _propertyUpdaters = new Dictionary<string, Func<int, string, Response>>
        {
            {"status", UpdateDeploymentStatus }
        };

        public UpdateDeployment()
        {
            Patch[Routing.Deployments.PATCH] = _ =>
            {
                var patchDetails = this.Bind<PatchModel>();
                return _propertyUpdaters[patchDetails.PropertyName](_.DeploymentId, patchDetails.PropertyValue);
            };
        }

        static Response UpdateDeploymentStatus(int deploymentId, string deploymentStatus)
        {
            Response response = HttpStatusCode.NotFound;
            DeploymentStatus status = (DeploymentStatus)Enum.Parse(typeof(DeploymentStatus), deploymentStatus);
            new DeploymentStatusUpdaterService(
                () => response = HttpStatusCode.NoContent,
                new DeploymentExistenceChecker(),
                new DeploymentStatusUpdater()).UpdateStatus(deploymentId, status);
            return response;
        }
    }
}