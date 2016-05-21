using System.Web;
using Nancy;
using Nancy.Conventions;
using System.Reflection;
using System;
using System.IO;
using Nancy.Bootstrapper;
using Nancy.ErrorHandling;
using Nancy.Responses;

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

    public class Test : NancyModule
    {
        public Test ()
        {
            Get ["/test"] = _ => View ["index.html"];   
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
