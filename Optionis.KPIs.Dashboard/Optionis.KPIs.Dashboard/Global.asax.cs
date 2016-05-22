using System.Web;
using Nancy;
using Nancy.Conventions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Optionis.KPIs.Dashboard
{
    public class Global : HttpApplication
    {
        protected void Application_Start()
        {
            JsonConvert.DefaultSettings = (() => {
                return new JsonSerializerSettings {
                    ContractResolver = new CamelCasePropertyNamesContractResolver ()
                };
            });
            
#if DEBUG
            StaticConfiguration.DisableErrorTraces = false;
#endif
        }
    }

    public class CustomBoostrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureConventions(NancyConventions conventions)
        {
            base.ConfigureConventions (conventions);

            conventions.StaticContentsConventions.AddDirectory ("Scripts", @"Scripts");
            conventions.StaticContentsConventions.AddDirectory ("Content", @"Content");
            conventions.StaticContentsConventions.AddDirectory ("/", @"/Content");
        }
    }
}
