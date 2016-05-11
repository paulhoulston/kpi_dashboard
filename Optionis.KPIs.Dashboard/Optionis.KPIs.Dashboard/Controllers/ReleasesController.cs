using System.Web.Http;
using Optionis.KPIs.Adapters;
using Optionis.KPIs.Dashboard.Core;
using System.Net.Http;
using System.Net;
using System;

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

        public HttpResponseMessage Post(CreateReleaseModel releaseToCreate)
        {
            return Request.CreateResponse (HttpStatusCode.Created, new CreateReleaseModel{
                Title = "Scheduled release for ClearSky",
                CreatedBy = "Paul Houlston",
                Comments = "A test release for latest functionality",
                Issues = new []
                {
                    new CreateReleaseModel.Issue
                    {
                        Id = "49995",
                        Link = "http://blah.com/49995",
                        Title = "A CR for an issue"
                    }
                },
                Deployments = new []
                {
                    new CreateReleaseModel.Deployment
                    {
                        Due = DateTime.Today.AddDays(1),
                        Status = CreateReleaseModel.DeploymentStatus.Pending
                    }
                }
            });
            //return Request.CreateResponse (HttpStatusCode.Created);
        }

        public class CreateReleaseModel
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

            public enum DeploymentStatus
            {
                Pending = 0,
                Success = 1,
                Failed = 2,
                Aborted = 3
            }

            public string Title{ get; set; }
            public string CreatedBy{ get; set; }
            public string Comments{ get; set; }
            public Issue[] Issues{get;set;}
            public Deployment[] Deployments{ get; set; }
        }
    }
}

