using System.Collections.Generic;
using Optionis.KPIs.Dashboard.Models;
using Optionis.KPIs.Dashboard.Core;
using SQLite;
using System;
using System.IO;

namespace Optionis.KPIs.Adapters
{
    class ReleasesRepository : ReleasesLister.IStoreReleases
    {
        static string DbFile
        {
            get { return Environment.CurrentDirectory + "\\SimpleDb.db"; }
        }

        public IEnumerable<Release> GetAll ()
        {
            if (!File.Exists (DbFile)) {
                using (var cnn = new SQLiteConnection (DbFile)) {
                    cnn.Table<Release> ();
                }
            }

            IEnumerable<Release> releases;
            using (var cnn = new SQLiteConnection(DbFile)) {
                releases = cnn.Query<Release> ("SELECT Id FROM Release");
                cnn.Close ();
            }
            return releases;
        }
    }
}
