using System.Web.Http;
using Optionis.KPIs.Adapters;
using System.Net.Http;
using System.Net;
using System;
using Optionis.KPIs.Dashboard.Application;
using System.Linq;
using Optionis.KPIs.Dashboard.Attributes;

namespace Optionis.KPIs.Dashboard
{
    public class CreateReleaseController : ApiController
    {
        [CreateRelease]
        public HttpResponseMessage Post(ReleaseToCreate releaseToCreate)
        {
            HttpResponseMessage response = null;
            new ReleseCreationService (
                new ReleasesRepository(),
                new UserRepository(),
                validationError => response = OnValidationError (validationError),
                releaseId => response = OnReleaseCreated (releaseId)
            ).Create (ConvertRelease (releaseToCreate));

            return response;
        }

        HttpResponseMessage OnReleaseCreated (int releaseId)
        {
            return Request.CreateResponse (HttpStatusCode.Created, new {
                self = string.Format ("releases/{0}", releaseId)
            });
        }

        HttpResponseMessage OnValidationError (ReleseCreationService.ValidationError validationError)
        {
            return Request.CreateResponse (HttpStatusCode.BadRequest, new {
                Error = new {
                    Code = validationError,
                    Message = ""
                }
            });
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
                Version = releaseToCreate.Version
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
