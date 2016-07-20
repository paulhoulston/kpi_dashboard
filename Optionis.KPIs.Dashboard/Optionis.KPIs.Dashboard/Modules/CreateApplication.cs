using Nancy;
using Nancy.ModelBinding;
using Optionis.KPIs.Dashboard.Application;
using System.Collections.Generic;
using Optionis.KPIs.DataAccess;
using Optionis.KPIs.Dashboard.Modules.Routes;

namespace Optionis.KPIs.Dashboard.Modules
{
    public class CreateApplication : NancyModule
    {
        readonly static IDictionary<ApplicationCreationService.ValidationError, string> _errorMessageLookup = new Dictionary<ApplicationCreationService.ValidationError, string> {
            { ApplicationCreationService.ValidationError.ApplicationIsNull, "The application is null" },
            { ApplicationCreationService.ValidationError.ApplicationNameNotSet, "The application name must not be empty" },
            { ApplicationCreationService.ValidationError.ApplicationNameTooLong, "The application name cannot exceed 255 characters" }
        };

        class ApplicationToCreate
        {
            public string ApplicationName { get; set; }
        }

        public CreateApplication()
        {
            Post[Routing.Applications.ROUTE] = _ =>
            {
                var request = this.Bind<ApplicationToCreate>();
                return PerformPost(request);
            };
        }

        Response PerformPost(ApplicationToCreate application)
        {
            Response response = null;
            new ApplicationCreationService(
                new ApplicationCreator(),
                validationError => response = OnValidationError(validationError),
                applicationId => response = OnApplicationCreated(applicationId)
            ).Create(new ApplicationCreationService.Application
            {
                ApplicationName = application.ApplicationName
            });

            return response;
        }

        Response OnValidationError(ApplicationCreationService.ValidationError validationError)
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

        Response OnApplicationCreated(int applicationId)
        {
            return Response.AsJson(new
            {
                links = new
                {
                    self = Routing.Applications.Get(applicationId)
                }
            }, HttpStatusCode.Created);
        }
    }
}