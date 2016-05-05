using System.Collections.Generic;
using Optionis.KPIs.Dashboard.Models;
using Optionis.KPIs.Dashboard.Core;

namespace Optionis.KPIs.Adapters
{
    class ReleasesRepository : ReleasesLister.IStoreReleases
    {
        public IEnumerable<Release> GetAll ()
        {
            return new Release[]{ };
        }
    }
}
