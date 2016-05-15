using System.Web.Http;
using Optionis.KPIs.Adapters;
using Optionis.KPIs.Dashboard.Core;
using System.Net.Http;
using System.Net;
using System;
using Optionis.KPIs.Dashboard.Application;
using System.Linq;

namespace Optionis.KPIs.Dashboard
{
    public class ReleasesController : ApiController
    {
        public dynamic Get()
        {
            return new
            {
                Releases = new ReleasesLister(new ReleasesRepository()).List()
            };
        }

        public HttpResponseMessage Get(int id)
        {
            return Request.CreateResponse (HttpStatusCode.OK, new Release{
                Id = id,
                Title = "Scheduled release for ClearSky",
                CreatedBy = "Paul Houlston",
                Created = DateTime.Now.AddHours(-1.4),
                Comments = "A test release for latest functionality",
                Issues = new []
                {
                    new Release.Issue
                    {
                        Id = "49995",
                        Link = "http://blah.com/49995",
                        Title = "A CR for an issue"
                    }
                },
                Deployments = new []
                {
                    new Release.Deployment
                    {
                        Due = DateTime.Today.AddDays(1),
                        Status = DeploymentStatus.Pending
                    }
                }
            });
        }

        public HttpResponseMessage Post(ReleaseToCreate releaseToCreate)
        {
            HttpResponseMessage response = null;
            new ReleseCreationService (
                null,
                null,
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

        public class Release
        {
            public class Issue
            {
                public string Id{ get; set; }
                public string Link{ get; set; }
                public string Title{ get; set; }
            }

            public class Deployment
            {
                public DateTime Due{ get; set; }
                public DeploymentStatus Status { get; set; }
            }
                
            public int Id { get; set; }
            public string Title{ get; set; }
            public string CreatedBy{ get; set; }
            public DateTime Created{ get; set; }
            public string Comments{ get; set; }
            public Issue[] Issues{get;set;}
            public Deployment[] Deployments{ get; set; }
        }
    }
}

