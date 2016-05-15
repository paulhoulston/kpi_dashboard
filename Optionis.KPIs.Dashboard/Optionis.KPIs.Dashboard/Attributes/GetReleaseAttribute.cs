namespace Optionis.KPIs.Dashboard.Attributes
{
    class GetReleaseAttribute : HttpVerbConstrainedRouteAttribute.GetRouteAttribute
    {
        public GetReleaseAttribute()
            : base("releases/{id}")
        {
        }
    }
}
