using System;
using Nancy;
using System.Threading.Tasks;
using System.Threading;

namespace Optionis.KPIs.Dashboard
{
    public class GetRelease : NancyModule
    {
        public GetRelease ()
        {
            Get["/releases/{id}", runAsync: true] = async (parameters, token) => await PerformGet (parameters, token);
        }

        async Task<Release> PerformGet(dynamic parameters, CancellationToken _)
        {
            return new Release{
                Id = parameters.id,
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
            };
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

