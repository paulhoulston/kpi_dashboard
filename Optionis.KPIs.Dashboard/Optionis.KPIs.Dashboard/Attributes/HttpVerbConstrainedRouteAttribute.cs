using System.Net.Http;
using System.Web.Http.Routing;
using System.Collections.Generic;

namespace Optionis.KPIs.Dashboard.Attributes
{
    abstract class HttpVerbConstrainedRouteAttribute : RouteFactoryAttribute
    {
        protected HttpVerbConstrainedRouteAttribute(string template, HttpMethod method)
            : base(template)
        {
            Method = method;
        }

        public HttpMethod Method { get; private set; }

        public override IDictionary<string, object> Constraints
        {
            get
            {
                return new HttpRouteValueDictionary
                {
                    {"method", new HttpVerbConstraint(Method)}
                };
            }
        }

        public class PostRouteAttribute : HttpVerbConstrainedRouteAttribute
        {
            public PostRouteAttribute(string template) : base(template, HttpMethod.Post) { }
        }

        public class PutRouteAttribute : HttpVerbConstrainedRouteAttribute
        {
            public PutRouteAttribute(string template) : base(template, HttpMethod.Put) { }
        }

        public class GetRouteAttribute : HttpVerbConstrainedRouteAttribute
        {
            public GetRouteAttribute(string template) : base(template, HttpMethod.Get) { }
        }

        public class DeleteRouteAttribute : HttpVerbConstrainedRouteAttribute
        {
            public DeleteRouteAttribute(string template) : base(template, HttpMethod.Delete) { }
        }
    }
}

