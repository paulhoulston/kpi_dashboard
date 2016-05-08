using SQLite;
using System;
using System.IO;
using Optionis.KPIs.Dashboard.Adapters.DatabaseModels;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

namespace Optionis.KPIs.Adapters
{
    class SqliteWrapper
    {
        static readonly object _locker = new object ();

        static string DbFile
        {
            get { return Environment.CurrentDirectory + "\\SimpleDb.db"; }
        }

        public SqliteWrapper ()
        {
            if (!File.Exists (DbFile)) {
                lock (_locker) {
                    if (!File.Exists (DbFile)) {
                        using (var cnn = new SQLiteConnection (DbFile)) {
                            // Create the tables for the database models
                            Assembly
                                .GetExecutingAssembly ()
                                .GetTypes ()
                                .Where (type => !type.IsInterface && type.GetInterface (typeof(IAmADatabaseModel).Name) != null)
                                .ToList ()
                                .ForEach (type => cnn.CreateTable (type));
                        }
                    }
                }
            }
        }

        public SQLiteConnection Connection()
        {
            return new SQLiteConnection (DbFile);
        }
    }
}
