using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;

namespace Optionis.KPIs.DataAccess.Database
{
    class SqlQueries
    {
        public static readonly IDictionary<Query, string> Queries;

        public enum Query
        {
            GetReleaseById,
            InsertDeployment,
            InsertIssue,
            InsertRelease,
        }

        static SqlQueries()
        {
            var queries = new Dictionary<Query, string>();
            foreach (var query in Enum.GetNames(typeof(Query)))
            {
                var resourceName = string.Format("Optionis.KPIs.DataAccess.Database.{0}.sql", query);
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                using (StreamReader reader = new StreamReader(stream))
                {
                    queries.Add((Query)Enum.Parse(typeof(Query), query), reader.ReadToEnd());
                }
            }
            Queries = new ReadOnlyDictionary<Query, string>(queries);
        }
    }
}
