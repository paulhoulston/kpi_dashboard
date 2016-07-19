using Nancy;
using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.Dashboard.Modules.Routes;
using Optionis.KPIs.Dashboard.Application.Tests;
using System.Linq;

namespace Optionis.KPIs.Dashboard
{
    public class ListApplications : NancyModule
    {
        public ListApplications()
        {
            Get[Routing.Applications.ROUTE] = _ =>
            {
                dynamic result = null;
                new ApplicationListingService(
                    applications =>
                result = new
                {
                    links = new { self = Routing.Applications.ROUTE },
                    applications = applications.OrderBy(app => app.Name)
                }, new ApplicationLister()).List();
                return result;
            };
        }
    }
}