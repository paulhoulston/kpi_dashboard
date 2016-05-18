using Nancy;
using System.Threading.Tasks;
using System.Threading;

namespace Optionis.KPIs.Dashboard
{
    public class CreateRelease : NancyModule
    {
        public CreateRelease ()
        {
            Post["/releases", runAsync: true] = async (parameters, token) => await PerformPost (parameters, token);
        }

        async Task<dynamic> PerformPost (dynamic parameters, CancellationToken token)
        {
            return new {
                self = "/releases/1"
            };
        }

        /*public dynamic Post(ReleaseToCreate releaseToCreate)
        {
            HttpResponseMessage response = null;
            response = Request.CreateResponse (HttpStatusCode.OK, releaseToCreate);

            new ReleseCreationService (
                new ReleasesRepository(),
                new UserRepository(),
                validationError => response = OnValidationError (validationError),
                releaseId => response = OnReleaseCreated (releaseId)
            ).Create (ConvertRelease (releaseToCreate));

            return response;
            return null;
        }*/

/*        HttpResponseMessage OnReleaseCreated (int releaseId)
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
        }*/
    }
    
}
