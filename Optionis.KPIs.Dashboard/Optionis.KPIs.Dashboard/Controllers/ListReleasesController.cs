using Nancy;

namespace Optionis.KPIs.Dashboard
{
    public class ListReleasesController : NancyModule
    {
        public ListReleasesController ()
        {
            Get ["/releases"] = _ => new
            {
                Releases = new dynamic[]{ }
            };
        }
    }
}
