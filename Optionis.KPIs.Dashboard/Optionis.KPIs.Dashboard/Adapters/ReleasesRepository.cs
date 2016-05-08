using System.Collections.Generic;
using Optionis.KPIs.Dashboard.Core;
using SQLite;
using Optionis.KPIs.Dashboard.Adapters.DatabaseModels;

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
