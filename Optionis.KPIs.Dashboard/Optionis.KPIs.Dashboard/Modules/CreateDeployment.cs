using System.Collections.Generic;
using Nancy;
using Nancy.ModelBinding;
using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.Dashboard.Modules.Routes;
using Optionis.KPIs.DataAccess;

namespace Optionis.KPIs.Dashboard.Modules
{
    public class CreateDeployment : NancyModule
    {
        static readonly IDictionary<DeploymentCreationService.ValidationError, string> _errorMessageLookup = new Dictionary<DeploymentCreationService.ValidationError, string> {
            { DeploymentCreationService.ValidationError.None,"No error" },
            { DeploymentCreationService.ValidationError.InvalidVersionNumber, "Version number is invalid. It must be in the format 1.0.0.0" },
            { DeploymentCreationService.ValidationError.InvalidDeploymentDate, "The deployment date cannot be more than 90 days in the past" },
            { DeploymentCreationService.ValidationError.InvalidComments, "The comments must be 1000 characters or less" }
        };

        public CreateDeployment ()
        {
            Post[Routing.Deployments.ROUTE] = parameters => {
                var request = this.Bind<DeploymentCreationService.Deployment>();
                return PerformPost(request);
            };
        }

        Response PerformPost(DeploymentCreationService.Deployment deployment)
        {
            Response response = null;
            new DeploymentCreationService(
                validationError => response = new SerializedError(validationError, _errorMessageLookup[validationError], Response).ErrorResponse(),
                releaseId => response = OnDeploymentCreated(releaseId),
                new DeploymentCreator()
            ).CreateDeployment(deployment);

            return response;
        }

        Response OnDeploymentCreated(int releaseId)
        {
            return Response.AsJson(new
            {
                links = new
                {
                    self = Routing.Releases.Get(releaseId)
                }
            }, HttpStatusCode.Created);
        }
    }
}

