using Nancy;
using Optionis.KPIs.Common;
using Optionis.KPIs.Dashboard.Application;

namespace Optionis.KPIs.Dashboard
{
    public class DeleteDeployment : NancyModule
    {
        public DeleteDeployment ()
        {
            Delete [Routing.Deployments.DELETE] = _ => {
                new DeploymentDeletionService (
                    new DeploymentRemover(),
                    new ReleaseRemover(),
                    new ReleaseHasAssignedDeploymentsChecker(),
                    new DeploymentExistenceChecker()).DeleteDeployment (_.Id);
                return HttpStatusCode.NoContent;
            };
        }
    }
}

