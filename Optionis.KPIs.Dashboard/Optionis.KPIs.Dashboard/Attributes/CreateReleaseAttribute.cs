namespace Optionis.KPIs.Dashboard.Attributes
{
    class CreateReleaseAttribute : HttpVerbConstrainedRouteAttribute.PostRouteAttribute
    {
        public CreateReleaseAttribute()
            : base("releases")
        {
        }
    }
}

