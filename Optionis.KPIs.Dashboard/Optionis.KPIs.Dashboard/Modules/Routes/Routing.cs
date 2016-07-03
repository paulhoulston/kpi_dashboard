namespace Optionis.KPIs.Dashboard.Modules.Routes
{
    public static class Routing
    {
        public static class Releases
        {
            public const string ROUTE = "/releases";
            public const string GET = "/releases/{id}";

            public static string Get(int id)
            {
                return GET.Replace (@"{id}", id.ToString ());
            }
        }

        public static class Issues
        {
            public const string GET = "/releases/{releaseId}/issues/{issueId}";

            public static string Get(int releaseId, int issueId)
            {
                return GET.Replace (@"{issueId}", issueId.ToString ()).Replace (@"{releaseId}", releaseId.ToString ());
            }
        }

        public static class Deployments
        {
            public const string ROUTE = "/releases/{releaseId}/deployments";
            public const string GET = "/releases/{releaseId}/deployments/{deploymentId}";
            public const string DELETE = "/releases/{releaseId}/deployments/{deploymentId}";
            public const string PATCH = "/releases/{releaseId}/deployments/{deploymentId}";

            public static string Get(int releaseId, int deploymentId)
            {
                return GET.Replace (@"{deploymentId}", deploymentId.ToString ()).Replace (@"{releaseId}", releaseId.ToString ());
            }

            public static string Add(int releaseId)
            {
                return ROUTE.Replace (@"{releaseId}", releaseId.ToString ());
            }                
        }

        public static class DeploymentStatuses
        {
            public const string ROUTE = "/deployments/statuses";
        }

        public static class Users
        {
            public const string ROUTE = "/users";
            public const string GET = "/users/{id}";

            public static string Get(int id)
            {
                return GET.Replace (@"{id}", id.ToString ());
            }
        }
    }
}

