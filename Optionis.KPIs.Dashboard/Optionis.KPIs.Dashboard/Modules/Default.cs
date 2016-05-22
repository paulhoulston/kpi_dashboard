using Nancy;

namespace Optionis.KPIs.Dashboard.Modules
{
    public class Default : NancyModule
    {
        public Default ()
        {
            Get [@"/"] = _ => Response.AsFile ("Content/index.html", "text/html");
        }
    }
}
