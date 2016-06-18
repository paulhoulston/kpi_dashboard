using Nancy;
using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.Dashboard.Adapters;
using Optionis.KPIs.Common;

namespace Optionis.KPIs.Dashboard.Modules
{
    
    public class ListUsers : NancyModule
    {
        public ListUsers ()
        {
            Get [Routing.Users.ROUTE] = _ => {
                dynamic response = null;
                new UserListingService (
                    new UserLister (),
                    users => response = new { users }).List ();
                return response;
            };
        }
    }}
