using System.Net.Http;
using System.Web.Http.Routing;
using System.Collections.Generic;

namespace Optionis.KPIs.Dashboard.Attributes
{
    class HttpVerbConstraint : IHttpRouteConstraint
    {
        public HttpMethod Method { get; private set; }

        public HttpVerbConstraint(HttpMethod method)
        {
            Method = method;
        }

        public bool Match(HttpRequestMessage request,
            IHttpRoute route,
            string parameterName,
            IDictionary<string, object> values,
            HttpRouteDirection routeDirection)
        {
            return request.Method == Method;
        }
    }    
}
