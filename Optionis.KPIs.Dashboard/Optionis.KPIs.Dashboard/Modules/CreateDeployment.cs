using System.Collections.Generic;
using Nancy;
using Nancy.ModelBinding;
using Optionis.KPIs.Common;
using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.DataAccess;

namespace Optionis.KPIs.Dashboard
{
    public class CreateDeployment : NancyModule
    {
        static readonly IDictionary<DeploymentCreationService.ValidationError, string> _errorMessageLookup = new Dictionary<DeploymentCreationService.ValidationError, string> {
            { DeploymentCreationService.ValidationError.None,"No error" },
            { DeploymentCreationService.ValidationError.InvalidVersionNumber, "Version number is invalid. It must be in the format 1.0.0.0" },
            { DeploymentCreationService.ValidationError.InvalidDeploymentDate, "The deployment date cannot be more than 30 days in the past" }
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
                validationError => response = OnValidationError(validationError),
                releaseId => response = OnDeploymentCreated(releaseId),
                new DeploymentCreator()
            ).CreateDeployment(deployment);

            return response;
        }

        Response OnValidationError(DeploymentCreationService.ValidationError validationError)
        {
            return Response.AsJson(new
            {
                Error = new
                {
                    Code = validationError.ToString(),
                    Message = _errorMessageLookup[validationError]
                }
            }, HttpStatusCode.BadRequest);
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

