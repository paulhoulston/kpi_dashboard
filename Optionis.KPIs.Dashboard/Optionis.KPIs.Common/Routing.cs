namespace Optionis.KPIs.Common
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
            public const string ROUTE = "/issues";
            public const string GET = "/issues/{id}";

            public static string Get(int id)
            {
                return GET.Replace (@"{id}", id.ToString ());
            }
        }

        public static class Deployments
        {
            public const string ROUTE = "/deployments";
            public const string GET = "/deployments/{id}";
            public const string DELETE = "/deployments/{id}";

            public static string Get(int id)
            {
                return GET.Replace (@"{id}", id.ToString ());
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

