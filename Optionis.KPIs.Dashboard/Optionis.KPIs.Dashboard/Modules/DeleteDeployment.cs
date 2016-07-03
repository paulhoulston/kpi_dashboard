using Nancy;
using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.Dashboard.Modules.Routes;
using Optionis.KPIs.DataAccess;

namespace Optionis.KPIs.Dashboard.Modules
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
                    new DeploymentExistenceChecker()).DeleteDeployment (_.DeploymentId);
                return HttpStatusCode.NoContent;
            };
        }
    }
}

