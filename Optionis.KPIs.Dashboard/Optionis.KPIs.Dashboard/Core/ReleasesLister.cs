using System.Collections.Generic;
using Optionis.KPIs.Dashboard.Models;
using System.Data.SQLite;
using System;
using System.IO;

namespace Optionis.KPIs.Dashboard.Core
{
    class ReleasesLister
    {
        readonly IStoreReleases _repository;

        public interface IStoreReleases
        {
            IEnumerable<Release> GetAll();
        }

        public ReleasesLister (IStoreReleases repository)
        {
            _repository = repository;
        }

        public IEnumerable<Release> List ()
        {
            if (!File.Exists(SqLiteBaseRepository.DbFile))
            {
                CreateDatabase();
            }

            return _repository.GetAll ();
        }

        static void CreateDatabase()
        {
            using (var cnn = SqLiteBaseRepository.SimpleDbConnection ()) {
                cnn.Open ();
            }
        }
    }

    public class SqLiteBaseRepository
    {
        public static string DbFile
        {
            get { return Environment.CurrentDirectory + "\\SimpleDb.sqlite"; }
        }

        public static SQLiteConnection SimpleDbConnection()
        {
            return new SQLiteConnection("Data Source=" + DbFile);
        }
    }
}
