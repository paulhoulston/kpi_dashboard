using Nancy;
using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.Adapters;
using System.Linq;
using System;
using Nancy.Responses.Negotiation;
using Newtonsoft.Json;
using Nancy.Extensions;
using System.Collections.Generic;

namespace Optionis.KPIs.Dashboard
{
    public class CreateRelease : NancyModule
    {
        static readonly IDictionary<ReleseCreationService.ValidationError, string> _errorMessageLookup = new Dictionary<ReleseCreationService.ValidationError, string> {
            { ReleseCreationService.ValidationError.None,"No error" },
            { ReleseCreationService.ValidationError.ObjectNotSet,"Object is null or not set" },
            { ReleseCreationService.ValidationError.InvalidVersion, "Version number is invalid. It must be in the format 1.0.0.0" },
            { ReleseCreationService.ValidationError.TitleNotSet,"A title for the release is required" },
            { ReleseCreationService.ValidationError.ApplicationNotSet,"An application name is required" },
            { ReleseCreationService.ValidationError.UserNotFound,"The created by user was not found" },
            { ReleseCreationService.ValidationError.InvalidIssue ,"The issues are invalid" },
            { ReleseCreationService.ValidationError.InvalidComments,"The comments must be less than 255 characters" },
            { ReleseCreationService.ValidationError.InvalidDeploymentDate, "The deployment date cannot be more than 30 days in the past" }
        };

        public CreateRelease ()
        {
            Post ["/releases"] = parameters => {
                dynamic request = JsonConvert.DeserializeObject<ReleaseToCreate>(Request.Body.AsString ());
                return PerformPost(request);
            };
        }

        Response PerformPost (ReleaseToCreate release)
        {
            Response response = null;

            new ReleseCreationService (
                new ReleasesRepository(),
                new UserRepository(),
                validationError => response = OnValidationError (validationError),
                releaseId => response = OnReleaseCreated (releaseId)
            ).Create (ConvertRelease (release));

            return response;
        }

        Response OnReleaseCreated (int releaseId)
        {
            return Response.AsJson (new {
                    self = string.Format ("releases/{0}", releaseId)
            }, HttpStatusCode.Created);
        }

        Response OnValidationError (ReleseCreationService.ValidationError validationError)
        {
            return Response.AsJson (new {
                Error = new {
                    Code = validationError.ToString (),
                    Message = _errorMessageLookup [validationError]
                }
            }, HttpStatusCode.BadRequest);
        }
            
        static ReleseCreationService.ReleaseToCreate ConvertRelease (ReleaseToCreate releaseToCreate)
        {
            return new ReleseCreationService.ReleaseToCreate {
                Application = releaseToCreate.Application,
                Comments = releaseToCreate.Comments,
                Created = DateTime.Now,
                CreatedBy = releaseToCreate.CreatedBy,
                Issues = ConvertIssues (releaseToCreate),
                Title = releaseToCreate.Title,
                Version = releaseToCreate.Version,
                DeploymentDate = releaseToCreate.DeploymentDate
            };
        }

        static ReleseCreationService.Issue[] ConvertIssues (ReleaseToCreate releaseToCreate)
        {
            return (releaseToCreate.Issues ?? new Issue[] { }).Select (issue => new ReleseCreationService.Issue {
                Id = issue.Id,
                Link = issue.Link,
                Title = issue.Title
            }).ToArray ();
        }
    }
}
