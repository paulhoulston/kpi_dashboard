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
            var id = 1;
            return Request.CreateResponse (HttpStatusCode.Created, new {
                self = string.Format ("releases/{0}", id)
            });
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

