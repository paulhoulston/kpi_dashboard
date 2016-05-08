using System.Collections.Generic;
using Optionis.KPIs.Dashboard.Core;
using Optionis.KPIs.Dashboard.ReadCache.DatabaseModels;
using Optionis.KPIs.Dashboard.ReadCache;

namespace Optionis.KPIs.Adapters
{
    class ReleasesRepository : ReleasesLister.IStoreReleases
    {
        public IEnumerable<Release> GetAll ()
        {
            using (var cnn = new SqliteWrapper ().Connection ()) {
                return cnn.Query<Release> ("SELECT Id FROM Release");
            }
        }
    }
}
