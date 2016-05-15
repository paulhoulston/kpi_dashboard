namespace Optionis.KPIs.Dashboard.Attributes
{
    class ListReleasesAttribute : HttpVerbConstrainedRouteAttribute.GetRouteAttribute
    {
        public ListReleasesAttribute()
            : base("releases")
        {
        }
    }
}
