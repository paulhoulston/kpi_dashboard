using System.Web;
using Nancy;

namespace Optionis.KPIs.Dashboard
{
    public class Global : HttpApplication
    {
        protected void Application_Start()
        {
#if DEBUG
            StaticConfiguration.DisableErrorTraces = false;
#endif
        }
    }
}
