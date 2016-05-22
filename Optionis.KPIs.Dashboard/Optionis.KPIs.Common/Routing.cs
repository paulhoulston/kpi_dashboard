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
    }
}

