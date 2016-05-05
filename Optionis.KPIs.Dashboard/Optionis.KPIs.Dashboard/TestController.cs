using System.Web.Http;

namespace Optionis.KPIs.Dashboard
{
    public class TestController : ApiController
    {
        public dynamic Get()
        {
            return new
            {
                message = "Hello world"
            };
        }
    }
}
